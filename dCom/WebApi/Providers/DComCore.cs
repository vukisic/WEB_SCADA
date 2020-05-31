using Common;
using Modbus.Connection;
using ProcessingModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebApi.Configuration;

namespace WebApi.Providers
{
	public class DComCore : IDisposable, IStateUpdater, IStorage
	{
		#region Fields
		public ObservableCollection<BasePointItem> Points { get; set; }
		private object lockObject = new object();
		private Thread timerWorker;
		private ConnectionState connectionState;
		private Acquisitor acquisitor;
		private AutoResetEvent acquisitionTrigger = new AutoResetEvent(false);
		private AutoResetEvent automationTrigger = new AutoResetEvent(false);
		private string logText;
		private StringBuilder logBuilder;
		private IFunctionExecutor commandExecutor;
		private IAutomationManager automationManager;
		private bool timerThreadStopSignal = true;
		private bool disposed = false;
		IConfiguration configuration;
		private IProcessingManager processingManager = null;
		
		Dictionary<int, IPoint> pointsCache = new Dictionary<int, IPoint>();
		#endregion Fields
		#region Properties

		public ConnectionState ConnectionState
		{
			get
			{
				return connectionState;
			}

			set
			{
				connectionState = value;
				if (connectionState == ConnectionState.CONNECTED)
				{
					if(automationManager == null)
						automationManager = new AutomationManager(this, processingManager, automationTrigger, configuration);
					automationManager.Start(configuration.DelayBetweenCommands);
				}
				else
				{
					automationManager.Stop();
				}
			}
		}

		public string LogText
		{
			get
			{
				return logText;
			}

			set
			{
				logText = value;
			}
		}

		#endregion Properties

		public DComCore()
		{
			configuration = new ConfigReader();
			commandExecutor = new FunctionExecutor(this, configuration);
			this.processingManager = new ProcessingManager(this, commandExecutor);
			this.acquisitor = new Acquisitor(acquisitionTrigger, this.processingManager, this, configuration);
			this.automationManager = new AutomationManager(this, processingManager, automationTrigger, configuration);
			InitializePointCollection();
			InitializeAndStartThreads();
			logBuilder = new StringBuilder();
			Thread.CurrentThread.Name = "Main Thread";
		}

		#region Private methods
		/// <summary>
		/// Initializes collection of Points
		/// </summary>
		private void InitializePointCollection()
		{
			Points = new ObservableCollection<BasePointItem>();
			foreach (var c in configuration.GetConfigurationItems())
			{
				for (int i = 0; i < c.NumberOfRegisters; i++)
				{
					BasePointItem pi = CreatePoint(c, i, this.processingManager);
					if (pi != null)
					{
						Points.Add(pi);
						pointsCache.Add(pi.PointId, pi as IPoint);
						processingManager.InitializePoint(pi.Type, pi.Address, pi.RawValue);
					}
				}
			}
		}

		/// <summary>
		/// Creates Point
		/// </summary>
		/// <param name="c">ConfigItem</param>
		/// <param name="i">Register number</param>
		/// <param name="processingManager">Processing manager instance</param>
		/// <returns>Created Point</returns>
		private BasePointItem CreatePoint(IConfigItem c, int i, IProcessingManager processingManager)
		{
			switch (c.RegistryType)
			{
				case PointType.DIGITAL_INPUT:
					return new DigitalInput(c, processingManager, this, configuration, i);

				case PointType.DIGITAL_OUTPUT:
					return new DigitalOutput(c, processingManager, this, configuration, i);

				case PointType.ANALOG_INPUT:
					return new AnalaogInput(c, processingManager, this, configuration, i);

				case PointType.ANALOG_OUTPUT:
					return new AnalogOutput(c, processingManager, this, configuration, i);

				default:
					return null;
			}
		}

		/// <summary>
		/// Initialization and start for all threads
		/// </summary>
		private void InitializeAndStartThreads()
		{
			InitializeTimerThread();
			StartTimerThread();
		}

		/// <summary>
		/// Initialization of timer thread
		/// </summary>
		private void InitializeTimerThread()
		{
			timerWorker = new Thread(TimerWorker_DoWork);
			timerWorker.Name = "Timer Thread";
		}


		/// <summary>
		/// Starts timer thread
		/// </summary>
		private void StartTimerThread()
		{
			timerWorker.Start();
		}

		/// <summary>
		/// Defines work of timer thread
		/// </summary>
		private void TimerWorker_DoWork()
		{
			while (timerThreadStopSignal)
			{
				if (disposed)
					return;

				acquisitionTrigger.Set();
				automationTrigger.Set();
				Thread.Sleep(1000);
			}
		}

		#endregion Private methods

		#region IStateUpdater implementation

		/// <summary>
		/// Upadates connection state
		/// </summary>
		/// <param name="currentConnectionState">New connection state</param>
		public void UpdateConnectionState(ConnectionState currentConnectionState)
		{
			ConnectionState = currentConnectionState;
		}

		/// <summary>
		/// Logs message
		/// </summary>
		/// <param name="message">Message to be logged</param>
		public void LogMessage(string message)
		{
			if (disposed)
				return;

			string threadName = Thread.CurrentThread.Name;
			lock (lockObject)
			{
				logBuilder.Append($"{DateTime.Now} [{threadName}]: {message}{Environment.NewLine}");
				LogText = logBuilder.ToString();
			}
		}

		#endregion IStateUpdater implementation

		/// <summary>
		/// IDisposable implementation
		/// </summary>
		public void Dispose()
		{
			disposed = true;
			timerThreadStopSignal = false;
			(commandExecutor as IDisposable).Dispose();
			this.acquisitor.Dispose();
			acquisitionTrigger.Dispose();
			automationManager.Stop();
			automationTrigger.Dispose();
		}

		/// <summary>
		/// Returns points for specifies identifiers
		/// </summary>
		/// <param name="pointIds">List of identifiers</param>
		/// <returns>List of points</returns>
		public List<IPoint> GetPoints(List<PointIdentifier> pointIds)
		{
			List<IPoint> retVal = new List<IPoint>(pointIds.Count);
			foreach (var pid in pointIds)
			{
				int id = PointIdentifierHelper.GetNewPointId(pid);
				IPoint p = null;
				if (pointsCache.TryGetValue(id, out p))
				{
					retVal.Add(p);
				}
			}
			return retVal;
		}

		/// <summary>
		/// Returns Log data
		/// </summary>
		/// <returns>Log Data</returns>
		public string GetLog()
		{
			return logBuilder.ToString().Replace(Environment.NewLine,"|");
		}

		/// <summary>
		/// Executes command for specified data
		/// </summary>
		/// <param name="pointId">Id of Point</param>
		/// <param name="address">Address of Point</param>
		/// <param name="value">Commanded Value</param>
		public void ExecuteCommand(int pointId,int address, int value)
		{
			if (pointsCache.TryGetValue(pointId, out IPoint point))
			{
				processingManager.ExecuteWriteCommand(point.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, (ushort)address, value);
			}
		}
	}
}
