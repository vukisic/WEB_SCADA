using Common;

namespace WebApi.Providers
{
	/// <summary>
	/// Digital Base Model
	/// </summary>
	internal abstract class DigitalBase : BasePointItem, IDigitalPoint
    {
        #region Fields
        private DState state;
        #endregion
        public DigitalBase(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}

        #region Properties
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
        #endregion
    }
}
