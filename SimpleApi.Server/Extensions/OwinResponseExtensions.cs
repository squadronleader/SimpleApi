using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApi.Server.Extensions
{
    public static class OwinResponseExtensions
    {
        public static void AddHeaders(this IOwinResponse response, Dictionary<string,string> headers)
        {
            if(headers == null) { return; }

            foreach (var header in headers)
            {
                //Note add keys lower case as url is case insenstive
                response.Headers.Add(header.Key, new string[] { header.Value });
            }
        }

        public static void WriteBody(this IOwinResponse response, string body)
        {
            var bodyBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(body);
            response.Body.Write(bodyBytes, 0, bodyBytes.Length);
        }
    }
}
