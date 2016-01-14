using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Owin;

namespace WebApi
{
    public static class CorrelateTelemetryItem
    {
        public static void InitializeFrom(this ITelemetry telemetry, IOwinContext context)
        {
            //context.Environment.Po

            telemetry.Context.User.Id = request.Context.User.Id;
        }
    }
}
