using Common;

namespace WebApi.Providers
{
    internal class DigitalInput : DigitalBase
	{
		public DigitalInput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i) 
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}

    }
}