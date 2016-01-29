using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.Owin;
using Owin;
using System.Linq;

namespace WebApi
{
    public static class InitializeRequestTelemetry
    {

        public static void ConfigureInitializers(this TelemetryConfiguration configuration, IAppBuilder app)
        {
            configuration.TelemetryInitializers.Add(new AzureRoleEnvironmentTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new DomainNameRoleInstanceTelemetryInitializer());

            configuration.TelemetryInitializers.Add(new ApplicationVersionTelemetryInitializer(app));
        }

        private class ApplicationVersionTelemetryInitializer : ITelemetryInitializer
        {
            private readonly string appVersion;

            public ApplicationVersionTelemetryInitializer(IAppBuilder app)
            {
                this.appVersion = app.Properties["Version"].ToString();
            }

            public void Initialize(ITelemetry telemetry)
            {
                if (!string.IsNullOrEmpty(this.appVersion))
                {
                    telemetry.Context.Component.Version = this.appVersion;
                }
            }
        }
    }
}
