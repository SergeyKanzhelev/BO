﻿using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace OwinSelfHostDemo
{
    public class AiExceptionLogger : ExceptionLogger
    {
        private readonly TelemetryClient client;

        public AiExceptionLogger()
        {
            client = new TelemetryClient();
        }

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                ExceptionTelemetry exception = new ExceptionTelemetry(context.Exception);
                exception.HandledAt = ExceptionHandledAt.Unhandled;
                    
                client.TrackException(exception);
            }
            base.Log(context);
        }
    }
}