using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

namespace WebApi
{
    public static class InitializeTelemetryChannel
    {
        public static void ConfigureChannel(this TelemetryConfiguration configuration)
        {
            configuration.TelemetryChannel = new ServerTelemetryChannel();

            configuration.TelemetryProcessorChainBuilder
                .UseAdaptiveSampling()
                .Build();
        }
    }
}
