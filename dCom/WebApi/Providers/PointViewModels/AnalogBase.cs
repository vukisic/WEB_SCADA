using Common;

namespace WebApi.Providers
{
	/// <summary>
	/// Analog Base Model
	/// </summary>
    public abstract class AnalogBase : BasePointItem, IAnalogPoint 
	{
        #region Fields
        private double eguValue;
        #endregion
        public AnalogBase(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}

        #region Properties
        public double EguValue
		{
			get
			{
				return eguValue;
			}

			set
			{
				eguValue = value;
			}
		}

		public override string DisplayValue
		{
			get
			{
				return EguValue.ToString();
			}
		}
        #endregion
    }
}
