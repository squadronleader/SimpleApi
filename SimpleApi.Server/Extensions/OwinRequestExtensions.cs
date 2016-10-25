using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApi.Server.Extensions
{
    public static class OwinRequestExtensions
    {
        public static string ReadBody(this IOwinRequest request)
        {
            return ReadBody(request.Body);
        }

        private static string ReadBody(Stream stream)
        {
            var bodyString = "";
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                memStream.Position = 0; //reset stream to start
                using (var reader = new StreamReader(memStream))
                {
                    bodyString = reader.ReadToEnd();
                }
            }

            return bodyString;
        }
    }
}
