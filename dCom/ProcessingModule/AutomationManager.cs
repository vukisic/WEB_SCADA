using Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProcessingModule
{
	/// <summary>
	/// Class containing logic for automated work.
	/// </summary>
	public class AutomationManager : IAutomationManager, IDisposable
	{
		private Thread automationWorker;
		private AutoResetEvent automationTrigger;
		private IStorage storage;
		private IProcessingManager processingManager;
		private int delayBetweenCommands;
		private IConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutomationManager"/> class.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="processingManager">The processing manager.</param>
		/// <param name="automationTrigger">The automation trigger.</param>
		/// <param name="configuration">The configuration.</param>
		public AutomationManager(IStorage storage, IProcessingManager processingManager, AutoResetEvent automationTrigger, IConfiguration configuration)
		{
			this.storage = storage;
			this.processingManager = processingManager;
			this.configuration = configuration;
			this.automationTrigger = automationTrigger;
			this.delayBetweenCommands = configuration.DelayBetweenCommands;
		}

		/// <summary>
		/// Initializes and starts the threads.
		/// </summary>
		private void InitializeAndStartThreads()
		{
			InitializeAutomationWorkerThread();
			StartAutomationWorkerThread();
		}

		/// <summary>
		/// Initializes the automation worker thread.
		/// </summary>
		private void InitializeAutomationWorkerThread()
		{
			automationWorker = new Thread(AutomationWorker_DoWork);
			automationWorker.Name = "Aumation Thread";
		}

		/// <summary>
		/// Starts the automation worker thread.
		/// </summary>
		private void StartAutomationWorkerThread()
		{
			if (automationWorker != null)
				automationWorker.Abort();
			automationWorker = new Thread(AutomationWorker_DoWork);
			automationWorker.Name = "Aumation Thread";
			automationWorker.Start();
			disposedValue = false;
		}

		/// <summary>
		/// Defines custom work for automation
		/// </summary>
		private void AutomationWorker_DoWork()
		{
			int counter = 0;
			List<IPoint> points;
			List<PointIdentifier> pointIdentifiers = new List<PointIdentifier>()
			{
				new PointIdentifier(PointType.DIGITAL_OUTPUT, 40),
				new PointIdentifier(PointType.DIGITAL_OUTPUT, 41),
				new PointIdentifier(PointType.DIGITAL_INPUT, 1000),
				new PointIdentifier(PointType.DIGITAL_INPUT, 1001),
				new PointIdentifier(PointType.ANALOG_INPUT, 2000),
				new PointIdentifier(PointType.ANALOG_INPUT, 2001),
				new PointIdentifier(PointType.ANALOG_OUTPUT, 3000),
				new PointIdentifier(PointType.ANALOG_OUTPUT, 3001),

			};
			while (!disposedValue)
			{
				if (counter == configuration.DelayBetweenCommands)
				{
					points = storage.GetPoints(pointIdentifiers);

					int DO1 = ((IDigitalPoint)points[0]).RawValue;
					int DO2 = ((IDigitalPoint)points[1]).RawValue;
					double AO1 = ((IAnalogPoint)points[6]).EguValue;
					double AO2 = ((IAnalogPoint)points[7]).EguValue;

					if (DO1 == 0)
					{
						DO1 = 1;
						processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress,
															pointIdentifiers[0].Address, DO1);
					}
						
					if (DO2 == 0)
					{
						DO2 = 1;
						processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress,
															pointIdentifiers[1].Address, DO2);
					}
						
					if (AO1 < 1000 || AO1 > 4500)
					{
						AO1 = 2000;
						processingManager.ExecuteWriteCommand(points[6].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress,
															pointIdentifiers[6].Address, (int)AO1);
					}
						
					if (AO2 < 1000 || AO2 > 4500)
					{
						AO2 = 2000;
						processingManager.ExecuteWriteCommand(points[7].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress,
															pointIdentifiers[7].Address, (int)AO2);
					}
					
					counter = 0;
				}
				automationTrigger.WaitOne();
				++counter;
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls


		/// <summary>
		/// Disposes the object.
		/// </summary>
		/// <param name="disposing">Indication if managed objects should be disposed.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
				}
				disposedValue = true;
			}
		}


		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// GC.SuppressFinalize(this);
		}

		/// <inheritdoc />
		public void Start(int delayBetweenCommands)
		{
			InitializeAndStartThreads();
		}

		/// <inheritdoc />
		public void Stop()
		{
			Dispose();
		}
		#endregion
	}
}
