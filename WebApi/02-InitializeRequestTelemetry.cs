using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Owin;
using System.Linq;

namespace WebApi
{
    public static class InitializeRequestTelemetry
    {
        public static void InitializeFrom(this RequestTelemetry rt, IOwinContext context)
        {
            rt.HttpMethod = context.Request.Method;
            rt.Url = context.Request.Uri;
            rt.ResponseCode = context.Response.StatusCode.ToString();
            rt.Success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;

            string[] xForwardedFor;
            rt.Context.Location.Ip = context.Request.Headers.TryGetValue("X-Forwarded-For", out xForwardedFor) ?
             xForwardedFor.FirstOrDefault() : (string)context.Request.Environment["server.RemoteIpAddress"];
        }
    }
}
