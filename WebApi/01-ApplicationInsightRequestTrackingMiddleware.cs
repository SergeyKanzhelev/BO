using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace WebApi
{
    public class ApplicationInsightRequestTrackingMiddleware : OwinMiddleware
    {
        private readonly TelemetryClient telemetryClient;

        public ApplicationInsightRequestTrackingMiddleware(OwinMiddleware next, TelemetryConfiguration telemetryConfiguration)
            : base(next)
        {
            telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public override async Task Invoke(IOwinContext context)
        {
            var operation = telemetryClient.StartRequestTracking(context, context.Request.Path.Value);

            try
            {
                var requestTelemetry = operation.Telemetry;

                await this.Next.Invoke(context);

                requestTelemetry.HttpMethod = context.Request.Method;
                requestTelemetry.Url = context.Request.Uri;
                requestTelemetry.ResponseCode = context.Response.StatusCode.ToString();
                requestTelemetry.Success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;

                requestTelemetry.InitializeContextFrom(context);
            }
            catch (Exception exc)
            {
                var telemetry = new ExceptionTelemetry(exc);
                telemetry.HandledAt = ExceptionHandledAt.Unhandled;

                telemetry.InitializeContextFrom(context);
                
                telemetryClient.TrackException(telemetry);
            }
            finally
            {
                telemetryClient.StopRequestTracking(operation);
            }
        }
    }
}
