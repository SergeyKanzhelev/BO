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

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {              
                // don't stop the server just yet
                Console.ReadLine();
            }
        }
    }
}
