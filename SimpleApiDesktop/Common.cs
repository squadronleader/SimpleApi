using SimpleApi.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimpleApi.Desktop
{
    public class Common
    {
        public static bool WriteNewFile(string fileName,string ext,EndpointConfiguration endpoint)
        {
            var json = JsonConvert.SerializeObject(endpoint);
            var path = Path.Combine("Content", fileName + ext);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, json);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ValidateJson(string jsonString)
        {
            var obj = JsonConvert.DeserializeObject(jsonString);
        }
    }
}
