﻿using Microsoft.ApplicationInsights.Extensibility;
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
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            appBuilder.Configure(config, version);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // alex's ikey: "a7010d58-fd1a-4d1f-b385-4d041d7c558c";
            TelemetryConfiguration.Active.InstrumentationKey = "c92059c3-9428-43e7-9b85-a96fb7c9488f";

            //appBuilder.EnableApplicationInsights();

            appBuilder.UseWebApi(config);
        }
    }
}