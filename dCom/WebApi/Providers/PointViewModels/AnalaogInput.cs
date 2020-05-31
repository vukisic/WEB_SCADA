using Common;

namespace WebApi.Providers
{
	/// <summary>
	/// Analog Input Model
	/// </summary>
	internal class AnalaogInput : AnalogBase
	{
		public AnalaogInput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}
	}
}