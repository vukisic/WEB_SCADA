using Common;

namespace WebApi.Providers
{
	/// <summary>
	/// Digital Input Model
	/// </summary>
    internal class DigitalInput : DigitalBase
	{
		public DigitalInput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i) 
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}

    }
}