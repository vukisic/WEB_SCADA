using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WebCommunication
{
    public interface IDataProvider
    {
        List<IConfigItem> GetConfig();
        string GetJsonConfig();
    }
}
