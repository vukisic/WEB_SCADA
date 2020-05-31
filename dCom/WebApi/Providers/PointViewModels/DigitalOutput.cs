using Common;
using System;

namespace WebApi.Providers
{
	/// <summary>
	/// Digital Output Model
	/// </summary>
    internal class DigitalOutput : DigitalBase
	{

		public DigitalOutput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, processingManager, stateUpdater, configuration, i)
		{
		}
    }
}