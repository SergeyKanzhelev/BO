using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
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

        public static IOperationHolder<RequestTelemetry> StartRequestTracking(this TelemetryClient telemetryClient, IOwinContext context, string name)
        {
            var operation = telemetryClient.StartOperation<RequestTelemetry>(name);

            var requestTelemetry = operation.Telemetry;
            context.Environment.Add("Microsoft.ApplicationInsights.RequestTelemetry", requestTelemetry);

            return operation;
        }

        public static void StopRequestTracking(this TelemetryClient telemetryClient, IOperationHolder<RequestTelemetry> operation)
        {
            telemetryClient.StopOperation(operation);
        }

    }
}
