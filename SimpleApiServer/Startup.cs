using Owin;
using SimpleApi.Server.Middleware;

namespace SimpleApi.Server
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
            app.Use(typeof(Logger));
            app.Use(typeof(SimpleRouter), "Content");
		}
	}
}
