using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmnSharpPlusCodes
{
    public class NewtonsoftJsonHelper
    {
        public static T ToObject<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string ToJsonFormat(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
