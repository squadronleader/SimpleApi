using Owin;
using SimpleApi.Middleware;

namespace SimpleApi
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
