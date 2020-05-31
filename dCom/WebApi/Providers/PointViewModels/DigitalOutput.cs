using Common;
using System;

namespace WebApi.Providers
{
    internal class DigitalOutput : DigitalBase
	{

		public DigitalOutput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}
    }
}