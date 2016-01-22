﻿using System;
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

            TelemetryConfiguration.Active.InstrumentationKey = "c92059c3-9428-43e7-9b85-a96fb7c9488f";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                Random rand = new Random();

                for (int i = 0; i < 100; i++)
                {
                    var response = client.GetAsync(baseAddress + "api/values").Result;

                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                    var idx = Convert.ToInt32(Math.Round(rand.NextDouble() * 100));
                    Thread.Sleep(TimeSpan.FromMilliseconds(idx));
                    
                    response = client.GetAsync(baseAddress + "api/values/" + idx ).Result;

                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                }
            }

            Console.ReadLine();
        }
    }
}
