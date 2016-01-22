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

            telemetry.Context.Operation.Id = requestTelemetry.Context.Operation.Id;
            telemetry.Context.Operation.ParentId = requestTelemetry.Id;
        }

        public static IOperationHolder<RequestTelemetry> StartRequestTracking(this TelemetryClient telemetryClient, IOwinContext context, string name)
        {
            var operation = telemetryClient.StartOperation<RequestTelemetry>(name);

            var requestTelemetry = operation.Telemetry;
            context.Environment.Add("Microsoft.ApplicationInsights.RequestTelemetry", requestTelemetry);

            if (context.Request.Headers.ContainsKey("x-ms-request-root-id"))
            {
                requestTelemetry.Context.Operation.Id = context.Request.Headers["x-ms-request-root-id"];
            }

            if (context.Request.Headers.ContainsKey("x-ms-request-id"))
            {
                requestTelemetry.Context.Operation.ParentId = context.Request.Headers["x-ms-request-id"];
            }

            return operation;
        }

        public static void StopRequestTracking(this TelemetryClient telemetryClient, IOperationHolder<RequestTelemetry> operation)
        {
            telemetryClient.StopOperation(operation);
        }

    }
}
