using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using System.Configuration;
using SimpleApiServer.Extensions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SimpleApiServer
{
	public class Program
	{
		private static ManualResetEvent _quitEvent = new ManualResetEvent(false);

		public static void Main(string[] args)
		{
            Trace.TraceInformation(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).FullName);
            //setup app defaults
            var port = ConfigurationManager.AppSettings.Get("port").ToIntOrDefault(Defaults.Port);

            //start new owin web server
			using (WebApp.Start<Startup>(string.Format("http://*:{0}", port)))
			{

			}
		}

        public static IDisposable Start()
        {
            Trace.TraceInformation(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).FullName);
            //setup app defaults
            var port = ConfigurationManager.AppSettings.Get("port").ToIntOrDefault(Defaults.Port);

            //start new owin web server
            return WebApp.Start<Startup>(string.Format("http://*:{0}", port));
        }
	}
}
