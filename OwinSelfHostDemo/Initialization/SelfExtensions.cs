﻿using Microsoft.ApplicationInsights.Extensibility;
using Owin;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights.Channel;
using System;
using OwinSelfHostDemo.Initialization;

namespace OwinSelfHostDemo
{
    public static class SelfExtensions
    {
        public static void Configure(this IAppBuilder app, HttpConfiguration config)
        {
            app.Properties["Version"] = typeof(Startup).Assembly.GetCustomAttributes(false)
                .OfType<AssemblyFileVersionAttribute>()
                .First()
                .Version;

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new FailedDepenendenciesTelemetryInitializer());
        }
    }
}
