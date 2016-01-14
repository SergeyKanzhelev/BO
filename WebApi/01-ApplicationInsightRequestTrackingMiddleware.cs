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
            var requestTelemetry = telemetryClient.StartOperation<RequestTelemetry>(context.Request.Path.Value);

            context.Environment.Add("Microsoft.ApplicationInsights.RequestTelemetry", requestTelemetry.Telemetry);

            try
            {
                await this.Next.Invoke(context);

                requestTelemetry.Telemetry.InitializeFrom(context);
            }
            catch(Exception exc)
            {
                telemetryClient.TrackException(exc);
            }
            finally
            {
                telemetryClient.StopOperation(requestTelemetry);
            }
        }
    }
}
