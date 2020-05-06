using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WebCommunication
{
    public interface IDataManager
    {
        object GetConfig();
        object GetValue(object obj);
        object GetValues();
    }
}
