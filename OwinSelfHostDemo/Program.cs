using System;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using Microsoft.ApplicationInsights.Extensibility;
using System.Threading;

namespace OwinSelfHostDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(new ThreadStart(() =>
            {
                using (WebApp.Start("http://localhost:9000/", (appBuilder) => { new Startup().Configuration(appBuilder, "1.0"); }))
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                    }
                }
            })).Start();


            new Thread(new ThreadStart(() =>
            {
                using (WebApp.Start("http://localhost:9001/", (appBuilder) => { new Startup().Configuration(appBuilder, "2.0"); }))
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                    }
                }
            })).Start();

            Console.ReadLine();
        }
    }
}
