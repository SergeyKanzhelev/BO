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
        public static void ListContextProperties(ITelemetry rt)
        {
            // Every telemetry item has a context associated with it
            // This context is used to describe request in a context of other telemetry items


            // Host context
            rt.Context.Cloud.RoleName = "Role Name";
            rt.Context.Cloud.RoleInstance = "Role Instance";


            // Application context
            rt.Context.Component.Version = "Application Version";


            // Application user context
            rt.Context.Location.Ip = "127.0.0.1";
            //rt.Context.Location.Country
            //rt.Context.Location.StateOrProvince
            //rt.Context.Location.City
            rt.Context.Operation.SyntheticSource = "Test in production";


            // Session context
            rt.Context.User.Id = "Anonymous User Id";
            rt.Context.Session.Id = "Anonymous Session Id";

            rt.Context.User.AccountId = "Account Id";
            rt.Context.User.AuthenticatedUserId = "Authenticated user id";


            // Operation context
            rt.Context.Operation.Id = "Root operatioin id";
            rt.Context.Operation.ParentId = "Parent Operation Id";
            rt.Context.Operation.Name = "Operation name";
        }

        public static void ConfigureInitializers(this TelemetryConfiguration configuration, IAppBuilder app)
        {
            //rt.Context.Cloud.RoleName = "Role Name";
            //rt.Context.Cloud.RoleInstance = "Role Instance";

            configuration.TelemetryInitializers.Add(new AzureRoleEnvironmentTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new DomainNameRoleInstanceTelemetryInitializer());

            //rt.Context.Component.Version = "Application Version";
            configuration.TelemetryInitializers.Add(new ApplicationVersionTelemetryInitializer(app));
        }

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

            rt.Context.User.AccountId = context.Request.Cookies["tenant"]?.ToString();
            rt.Context.User.AuthenticatedUserId = context.Request.User?.Identity?.Name;
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
