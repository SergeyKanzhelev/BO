using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace WebApi
{
    public static class InitializeTelemetryItem
    {
        public static void InitializeFrom(this ITelemetry telemetry, RequestTelemetry request)
        {
            telemetry.Context.User.Id = request.Context.User.Id;
        }
    }
}
