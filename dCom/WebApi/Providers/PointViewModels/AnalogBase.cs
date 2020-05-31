using Common;

namespace WebApi.Providers
{
    public abstract class AnalogBase : BasePointItem, IAnalogPoint 
	{
		private double eguValue;

		public AnalogBase(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}

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
	}
}
