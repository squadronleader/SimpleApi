using Owin;
using SimpleApiServer.Middleware;

namespace SimpleApiServer
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
