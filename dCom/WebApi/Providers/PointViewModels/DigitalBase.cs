using Common;

namespace WebApi.Providers
{
    internal abstract class DigitalBase : BasePointItem, IDigitalPoint
    {
		private DState state;

		public DigitalBase(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}

		public DState State
		{
			get
			{
				return state;
			}

			set
			{
				state = value;
			}
		}

		public override string DisplayValue
		{
			get
			{
				return State.ToString();
			}
		}

		protected override bool WriteCommand_CanExecute(object obj)
		{
			return false;
		}
	}
}
