using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Reflection;
using WebApi;

[assembly: OwinStartup(typeof(WebApplication3.Startup))]

namespace WebApplication3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Properties["Version"] = typeof(Startup).Assembly.GetCustomAttributes(false)
                .OfType<AssemblyFileVersionAttribute>()
                .First()
                .Version;

            ConfigureAuth(app);
            app.EnableApplicationInsights();
        }
    }
}
