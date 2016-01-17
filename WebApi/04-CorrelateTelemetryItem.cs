using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Owin;

namespace WebApi
{
    public static class CorrelateTelemetryItem
    {
        public static void InitializeFrom(this ITelemetry telemetry, IOwinContext context)
        {
            var requestTelemetry = (RequestTelemetry)context.Environment["Microsoft.ApplicationInsights.RequestTelemetry"];

            telemetry.Context.User.Id = requestTelemetry.Context.User.Id;
        }
    }
}
