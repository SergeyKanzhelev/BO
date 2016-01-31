using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinSelfHostDemo
{
    public class ValuesController : ApiController
    {
        private static Random rand = new Random();

        // GET api/values 
        public IEnumerable<string> Get()
        {
            var idx = Convert.ToInt32(Math.Round(rand.NextDouble() * 100));
            Thread.Sleep(TimeSpan.FromMilliseconds(idx));

            var client = new TelemetryClient();
            client.TrackTrace("trace get");

            if (rand.NextDouble() > 0.8)
            {
                throw new InvalidOperationException("Server performed an invalid operation");
            }
            else
            {
                return new string[] { "value1", "value2" };
            }
        }

        // GET api/values/5 
        public async Task<string> Get(int id)
        {
            var client = new TelemetryClient();
            client.TrackTrace("trace get/id");

            HttpClient httpClient = new HttpClient();

            if (id == 42)
            {
                return await httpClient.GetStringAsync("http://google.com/404/");
            }

            return await Task.FromResult("id: " + id);
        }
    }
}