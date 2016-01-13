using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
