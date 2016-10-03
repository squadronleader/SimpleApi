using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApiServer
{
    public class EndpointConfiguration
    {
        public string Url { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string,string> Headers { get; set; }
        public string Body { get; set; }
    }
}
