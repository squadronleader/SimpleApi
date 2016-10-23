using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApi.Server.Middleware
{
    public class Logger : OwinMiddleware
    {
        public Logger(OwinMiddleware next) : base(next) {}

        public override Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            Console.WriteLine(request.Method + " " + request.Uri.ToString());

            return Next.Invoke(context);
        }
    }
}
