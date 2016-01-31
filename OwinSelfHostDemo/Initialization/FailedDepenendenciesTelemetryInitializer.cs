using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinSelfHostDemo.Initialization
{
    public class FailedDepenendenciesTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is DependencyTelemetry)
            {
                var dependency = telemetry as DependencyTelemetry;
                if (dependency.ResultCode != "200")
                {
                    dependency.Success = false;
                }
            }
        }
    }
}
