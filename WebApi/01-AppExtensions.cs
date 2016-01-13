using Microsoft.ApplicationInsights.Extensibility;
using Owin;

namespace WebApi
{
    public static class AppExtensions
    {
        public static void EnableApplicationInsights(this IAppBuilder app, string instrumentationKey)
        {
            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = instrumentationKey;

            configuration.ConfigureChannel();

            app.Use<ApplicationInsightRequestTrackingMiddleware>(configuration);
        }
    }
}
