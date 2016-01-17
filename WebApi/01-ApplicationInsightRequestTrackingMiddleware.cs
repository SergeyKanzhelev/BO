using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using System;
using System.Runtime.Remoting.Messaging;
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
            telemetryClient.TrackEvent("Initialized");
        }

        public override async Task Invoke(IOwinContext context)
        {
            var operation = telemetryClient.StartOperation<RequestTelemetry>(context.Request.Path.Value);
            var requestTelemetry = operation.Telemetry;

            context.Environment.Add("Microsoft.ApplicationInsights.RequestTelemetry", requestTelemetry);

            CallContext.LogicalSetData("Test", operation);

            try
            {
                await this.Next.Invoke(context);

                requestTelemetry.HttpMethod = context.Request.Method;
                requestTelemetry.Url = context.Request.Uri;
                requestTelemetry.ResponseCode = context.Response.StatusCode.ToString();
                requestTelemetry.Success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;

                requestTelemetry.InitializeContextFrom(context);
            }
            catch(Exception exc)
            {
                telemetryClient.TrackException(exc);
            }
            finally
            {
                telemetryClient.StopOperation(operation);
            }
        }
    }
}
