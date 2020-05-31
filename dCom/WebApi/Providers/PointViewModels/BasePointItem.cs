using Common;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace WebApi.Providers
{
	/// <summary>
	/// Base for all other types of points
	/// </summary>
	public abstract class BasePointItem : IDataErrorInfo
	{
        #region Fields
        protected PointType type;
		protected ushort address;
		private DateTime timestamp = DateTime.Now;
		private string name = string.Empty;
		private ushort rawValue;
		private double commandedValue;
		protected AlarmType alarm;

		protected IProcessingManager processingManager;
		protected IConfiguration configuration;

		protected Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

		protected IStateUpdater stateUpdater;
		protected IConfigItem configItem;

		int pointId;
        #endregion 
        public BasePointItem(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
		{
			this.configItem = c;
			this.processingManager = processingManager;
			this.stateUpdater = stateUpdater;
			this.configuration = configuration;

			this.type = c.RegistryType;
			this.address = (ushort)(c.StartAddress + i);
			this.name = $"{configItem.Description} [{i}]";
			this.rawValue = configItem.DefaultValue;
			this.pointId = PointIdentifierHelper.GetNewPointId(new PointIdentifier(this.type, this.address));
		}

		#region Properties

		public PointType Type
		{
			get
			{
				return type;
			}

			set
			{
				type = value;
			}
		}

		/// <summary>
		/// Address of point on MdbSim Simulator
		/// </summary>
		public ushort Address
		{
			get
			{
				return address;
			}

			set
			{
				address = value;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return timestamp;
			}

			set
			{
				timestamp = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		public virtual string DisplayValue
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Value that is sent on MdbSim simulator
		/// </summary>
		public double CommandedValue
		{
			get
			{
				return commandedValue;
			}

			set
			{
				commandedValue = value;
			}
		}

		/// <summary>
		/// Raw value, read from MdbSim
		/// </summary>
		public virtual ushort RawValue
		{
			get
			{
				return rawValue;
			}
			set
			{
				rawValue = value;
			}
		}

		public IConfigItem ConfigItem
		{
			get
			{
				return configItem;
			}
		}

		#endregion Properties

		#region Input validation

		public string Error
		{
			get
			{
				return string.Empty;
			}
		}

		public AlarmType Alarm
		{
			get
			{
				return alarm;
			}

			set
			{
				alarm = value;
			}
		}

		public int PointId
		{
			get
			{
				return pointId;
			}
		}

		public string this[string columnName]
		{
			get
			{
				string message = string.Empty;
				if (columnName == "CommandedValue")
				{
					if (commandedValue > configItem.MaxValue)
					{
						message = $"Entered value cannot be greater than {configItem.MaxValue}.";
					}
					if (commandedValue < configItem.MinValue)
					{
						message = $"Entered value cannot be lower than {configItem.MinValue}.";
					}
				}
				return message;
			}
		}

		#endregion Input validation
	}
}