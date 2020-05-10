using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Serializers
{
    public class JsonSerializer<T> : IJsonSerializer<T>
    {
        public List<T> Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        public string Serialize(List<T> objs)
        {
            return JsonConvert.SerializeObject(objs, Formatting.Indented);
        }
    }
}
