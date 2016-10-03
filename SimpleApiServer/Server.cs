using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using System.Configuration;
using SimpleApiServer.Extensions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SimpleApiServer
{
	public class Server
	{
        public static IDisposable Start()
        {
            //Note this is a work around so that the OwinHttpListener is included when ref this project in other projects.
            //Its not copied by default because it not directly ref by anything so the build optimiser removes it.
            Trace.TraceInformation(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).FullName);
            //setup app defaults
            var port = ConfigurationManager.AppSettings.Get("port").ToIntOrDefault(Defaults.Port);

            //start new owin web server
            return WebApp.Start<Startup>(string.Format("http://*:{0}", port));
        }
	}
}
