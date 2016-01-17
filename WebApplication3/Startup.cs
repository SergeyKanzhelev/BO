using Microsoft.Owin;
using Owin;
using WebApi;

[assembly: OwinStartup(typeof(WebApplication3.Startup))]

namespace WebApplication3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.EnableApplicationInsights();
        }
    }
}
