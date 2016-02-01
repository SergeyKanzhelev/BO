using Microsoft.ApplicationInsights.Extensibility;
using Owin;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights.Channel;
using System;
using OwinSelfHostDemo.Initialization;
using Microsoft.Practices.Unity;

namespace OwinSelfHostDemo
{
    public static class SelfExtensions
    {
        public static void Configure(this IAppBuilder app, HttpConfiguration config, string version, TelemetryConfiguration configuration)
        {
            app.Properties["Version"] = version;

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger(configuration));

            var container = new UnityContainer();
            container.RegisterInstance<TelemetryConfiguration>(configuration);
            config.DependencyResolver = new UnityResolver(container);

            configuration.TelemetryInitializers.Add(new FailedDepenendenciesTelemetryInitializer());
        }
    }
}
