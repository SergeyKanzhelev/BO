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
            string baseAddress = "http://localhost:9000/";

            // alex's ikey: "a7010d58-fd1a-4d1f-b385-4d041d7c558c";
            TelemetryConfiguration.Active.InstrumentationKey = "c92059c3-9428-43e7-9b85-a96fb7c9488f";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {              
                // don't stop the server just yet
                Console.ReadLine();
            }
        }
    }
}
