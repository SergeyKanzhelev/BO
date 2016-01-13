using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    public static class AppExtensions
    {
        public static void EnableApplicationInsights(this IAppBuilder app)
        {
            app.Use<ApplicationInsightRequestTrackingMiddleware>();
        }
    }
}
