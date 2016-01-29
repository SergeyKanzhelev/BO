using System;
using System.Net.Http;
using System.Threading;

namespace OwinSelfHostDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Create HttpCient and make a request to api/values 
            HttpClient client = new HttpClient();

            Random rand = new Random();

            ThreadStart ts = new ThreadStart(() =>
            {
                while (true)
                {
                    var response = client.GetAsync(baseAddress + "api/values").Result;

                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                    var idx = Convert.ToInt32(Math.Round(rand.NextDouble() * 100));
                    Thread.Sleep(TimeSpan.FromMilliseconds(idx));

                    response = client.GetAsync(baseAddress + "api/values/" + idx).Result;

                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                }
            });

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(ts);
                t.Start();
            }
        }
    }
}
