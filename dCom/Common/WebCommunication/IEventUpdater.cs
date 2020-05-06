using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WebCommunication
{
    public interface IEventUpdater
    {
        event EventHandler<object> ValueUpdate;
        event EventHandler<object> ConfigUpdate;
        event EventHandler ConnectionUpdate;
    }
}
