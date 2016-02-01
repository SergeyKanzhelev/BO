using Microsoft.ApplicationInsights.Extensibility;
using Owin;
using OwinSelfHostSDK;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace OwinSelfHostDemo
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder, string version)
        {
            var configuration = TelemetryConfiguration.CreateDefault();
            // alex's ikey: "a7010d58-fd1a-4d1f-b385-4d041d7c558c";
            configuration.InstrumentationKey = "c92059c3-9428-43e7-9b85-a96fb7c9488f"; //BO
            //configuration.InstrumentationKey = "9d3ebb4f-7a11-4fb1-91ac-7ca8a17627eb"; //BO2

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            appBuilder.Configure(config, version, configuration);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //appBuilder.EnableApplicationInsights(configuration);

            appBuilder.UseWebApi(config);
        }
    }
}