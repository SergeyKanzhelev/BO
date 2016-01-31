using Microsoft.ApplicationInsights.Extensibility;
using Owin;

namespace WebApi
{
    public static class AppExtensions
    {
        public static void EnableApplicationInsights(this IAppBuilder app)
        {
            var configuration = TelemetryConfiguration.Active;

            configuration.ConfigureChannel();
            configuration.ConfigureInitializers(app);
            configuration.ConfigureTelemetryModules();

            app.Use<ApplicationInsightRequestTrackingMiddleware>(configuration);
        }
    }
}
