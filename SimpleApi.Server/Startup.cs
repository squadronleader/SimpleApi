using Owin;
using SimpleApi.Server.Handlers.Json;
using SimpleApi.Server.Middleware;
using System.Collections.Generic;

namespace SimpleApi.Server
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
            app.Use(typeof(Logger));
           // app.Use(typeof(SimpleRouter), "Content");
            app.Use(typeof(RouterHandler),
                new List<IRequestHandler> { new JsonRequestHandler() },
                "Content");
		}
	}
}
