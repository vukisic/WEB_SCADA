using Common;

namespace WebApi.Providers
{
	internal class AnalaogInput : AnalogBase
	{
		public AnalaogInput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}
	}
}