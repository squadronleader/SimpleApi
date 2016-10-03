using System;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;

namespace SimpleApi.Console
{
	public class Program
	{
		private static ManualResetEvent _quitEvent = new ManualResetEvent(false);

		static void Main(string[] args)
		{
			System.Console.CancelKeyPress += (sender, eArgs) =>
			{
				_quitEvent.Set();
				eArgs.Cancel = true;
			};

            //start new owin web server
			using (SimpleApi.Server.Server.Start())
			{
				System.Console.WriteLine(string.Format("Started Server"));
				_quitEvent.WaitOne();
			}
		}
	}
}
