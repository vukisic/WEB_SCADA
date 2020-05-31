using Common;
using System;

namespace WebApi.Providers
{
	/// <summary>
	/// Analog Output Model
	/// </summary>
	internal class AnalogOutput : AnalogBase
	{

		public AnalogOutput(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base (c, processingManager, stateUpdater, configuration, i)
		{		
		}
	}
}