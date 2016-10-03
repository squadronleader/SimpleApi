using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApi.Server.Extensions
{
    public static class CommonExtensions
    {
        public static int ToIntOrDefault(this string value,int defaultValue)
        {

            if(int.TryParse(value,out defaultValue))
            {
                return defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static TValue GetOrDefault<TKey,TValue>(this Dictionary<TKey,TValue> dictionary, TKey key)
        {
            if(dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(TValue);
        }
    }
}
