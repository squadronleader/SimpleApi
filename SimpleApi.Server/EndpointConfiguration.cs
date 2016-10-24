using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApi.Server
{
    public class EndpointConfiguration
    {
        public string Url { get; set; }
        public List<ResponseConfiguration> Responses { get; set; } = new List<ResponseConfiguration>();
        public ResponseConfiguration DefaultResponse { get; set; }
        public ResponseConfiguration ErrorResponse { get; set; }
        public dynamic ValidateRequest { get; set; }

        public EndpointConfiguration()
        {
            DefaultResponse = new ResponseConfiguration
            {
                HttpStatus = 501,
                Body = "No configured response found"
            };

            ErrorResponse = new ResponseConfiguration
            {
                HttpStatus = 400,
                Body = "Default Error Response"
            };
        }
    }

    public class ResponseConfiguration
    {
        public string ContentType { get; set; } = "plain/text";
        public Dictionary<string, string> Headers { get; set; }
        public int HttpStatus { get; set; } = 200;
        public string Body { get; set; } = "";
        public string OnlyIf { get; set; } = "";
        public List<ResponseRule> Rules { get; set; }
    }
     
    public class ResponseRule
    {
        public string PropertyPath { get; set; }
        public string Matches { get; set; }
    }
}
