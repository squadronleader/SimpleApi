using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using System.Configuration;
using SimpleApi.Extensions;
using System.Collections.Generic;

namespace SimpleApi
{
	public class Program
	{
		private static ManualResetEvent _quitEvent = new ManualResetEvent(false);

		static void Main(string[] args)
		{
            //setup app defaults
            var port = ConfigurationManager.AppSettings.Get("port").ToIntOrDefault(Defaults.Port);

			Console.CancelKeyPress += (sender, eArgs) =>
			{
				_quitEvent.Set();
				eArgs.Cancel = true;
			};

            //start new owin web server
			using (WebApp.Start<Startup>(string.Format("http://*:{0}", port)))
			{
				Console.WriteLine(string.Format("Started Server on port: {0}",port));
				_quitEvent.WaitOne();
			}
		}
	}
}
