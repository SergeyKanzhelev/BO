using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using System.Linq;

namespace WebApi
{
    public static class CorrelateTelemetryItem
    {
        public static void InitializeContextFrom(this ITelemetry rt, IOwinContext context)
        {
            rt.Context.Location.Ip = context.Request.RemoteIpAddress;

            string[] userAgent;
            if (context.Request.Headers.TryGetValue("User-Agent", out userAgent) && userAgent.FirstOrDefault() == "AlwaysOn")
            {
                rt.Context.Operation.SyntheticSource = "Azure AlwaysOn";
            }

            rt.Context.User.Id = context.Request.Cookies["ai_user"]?.Split("|".ToCharArray())[0];
            rt.Context.Session.Id = context.Request.Cookies["ai_session"]?.Split("|".ToCharArray())[0];
        }

        public static void InitializeFrom(this ITelemetry telemetry, IOwinContext context)
        {
            var requestTelemetry = (RequestTelemetry)context.Environment["Microsoft.ApplicationInsights.RequestTelemetry"];

            telemetry.Context.Operation.Id = requestTelemetry.Context.Operation.Id;
            telemetry.Context.Operation.ParentId = requestTelemetry.Id;
        }

        public static void StartRequestTracking(this TelemetryClient telemetryClient, IOwinContext context, RequestTelemetry requestTelemetry)
        {
            context.Environment.Add("Microsoft.ApplicationInsights.RequestTelemetry", requestTelemetry);

            if (context.Request.Headers.ContainsKey("x-ms-request-root-id"))
            {
                requestTelemetry.Context.Operation.Id = context.Request.Headers["x-ms-request-root-id"];
            }

            if (context.Request.Headers.ContainsKey("x-ms-request-id"))
            {
                requestTelemetry.Context.Operation.ParentId = context.Request.Headers["x-ms-request-id"];
            }
        }
    }
}
