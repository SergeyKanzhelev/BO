using Microsoft.ApplicationInsights.Extensibility;
using Owin;

namespace WebApi
{
    public static class AppExtensions
    {
        public static void EnableApplicationInsights(this IAppBuilder app)
        {
            // alex's ikey: "a7010d58-fd1a-4d1f-b385-4d041d7c558c";
            TelemetryConfiguration.Active.InstrumentationKey = "c92059c3-9428-43e7-9b85-a96fb7c9488f";

            var configuration = TelemetryConfiguration.Active;

            configuration.ConfigureChannel();
            configuration.ConfigureInitializers(app);
            configuration.ConfigureTelemetryModules();

            app.Use<ApplicationInsightRequestTrackingMiddleware>(configuration);
        }
    }
}
