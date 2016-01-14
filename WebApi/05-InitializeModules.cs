using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using System.Collections.Generic;

namespace WebApi
{
    public static class InitializeModules
    {
        private static List<object> telemetryModules = new List<object>();

        public static void ConfigureTelemetryModules(this TelemetryConfiguration configuration)
        {
            var performanceCounters = new PerformanceCollectorModule();
            performanceCounters.Counters.Add(new PerformanceCounterCollectionRequest(@"\Process\ID", "ID"));
            performanceCounters.Initialize(configuration);

            telemetryModules.Add(performanceCounters);

            var dependencies = new DependencyTrackingTelemetryModule();
            dependencies.Initialize(configuration);
           
            telemetryModules.Add(dependencies);
        }
    }
}
