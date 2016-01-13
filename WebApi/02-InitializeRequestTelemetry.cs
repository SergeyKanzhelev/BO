using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer;
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

            rt.Context.Component.Version = "Application Version";

            string[] xForwardedFor;
            rt.Context.Location.Ip = context.Request.Headers.TryGetValue("X-Forwarded-For", out xForwardedFor) ?
                xForwardedFor.FirstOrDefault() 
                : (string)context.Request.Environment["server.RemoteIpAddress"];

            //rt.Context.Operation.Id
            //rt.Context.Operation.ParentId
            //rt.Context.Operation.Name

            string[] userAgent;
            if (context.Request.Headers.TryGetValue("User-Agent", out userAgent) && userAgent.FirstOrDefault() == "AlwaysOn")
            {
                rt.Context.Operation.SyntheticSource = "Azure AlwaysOn";
            }


            rt.Context.User.Id = context.Request.Cookies["ai_user"]?.Split("|".ToCharArray())[0];
            rt.Context.Session.Id = context.Request.Cookies["ai_session"]?.Split("|".ToCharArray())[0];

            rt.Context.User.AccountId = context.Request.Cookies["tenant"]?.ToString();
            rt.Context.User.AuthenticatedUserId = context.Request.User?.Identity?.Name;
        }

        public static void ConfigureInitializers(this TelemetryConfiguration configuration)
        {
            //rt.Context.Cloud.RoleName = "Role Name";
            //rt.Context.Cloud.RoleInstance = "Role Instance";

            configuration.TelemetryInitializers.Add(new AzureRoleEnvironmentTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new DomainNameRoleInstanceTelemetryInitializer());

        }

    }
}
