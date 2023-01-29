using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Extensions
{
    public static class Serialization
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        public static T DeSerialize<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }       
    }
}