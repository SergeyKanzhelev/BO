using System;
using System.Net.Http;
using System.Threading;

namespace OwinSelfHostDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create HttpCient and make a request to api/values 
            HttpClient client = new HttpClient();

            Random rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        string baseAddress = "http://localhost:9000/";

                        var response = client.GetAsync(baseAddress + "api/values").Result;

                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                        var idx = Convert.ToInt32(Math.Round(rand.NextDouble() * 100));
                        Thread.Sleep(TimeSpan.FromMilliseconds(idx));

                        response = client.GetAsync(baseAddress + "api/values/" + idx).Result;

                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                    }
                })).Start();
            }


            for (int i = 0; i < 2; i++)
            {
                new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        string baseAddress = "http://localhost:9001/";

                        var idx = Convert.ToInt32(Math.Round(rand.NextDouble() * 100));
                        Thread.Sleep(TimeSpan.FromMilliseconds(idx));

                        var response = client.GetAsync(baseAddress + "api/values/" + idx).Result;

                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                    }
                })).Start();
            }

        }
    }
}
