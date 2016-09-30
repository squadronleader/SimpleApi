using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApi.Extensions
{
    public static class OwinResponseExtensions
    {
        public static void AddHeaders(this OwinResponse response, Dictionary<string,string> headers)
        {
            foreach (var header in headers)
            {
                //Note add keys lower case as url is case insenstive
                response.AddHeader(header.Key, header.Value);
            }
        }

        public static void WriteBody(this OwinResponse response, string body)
        {
            var bodyBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(body);
            response.Body.Write(bodyBytes, 0, bodyBytes.Length);
        }
    }
}
