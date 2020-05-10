using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Serializers
{
    public interface IJsonSerializer<T>
    {
        string Serialize(List<T> objs);
        List<T> Deserialize(string str);
    }
}
