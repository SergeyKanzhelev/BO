using Owin;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using WebApi;

namespace OwinSelfHostDemo
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Properties["Version"] = typeof(Startup).Assembly.GetCustomAttributes(false)
                .OfType<AssemblyFileVersionAttribute>()
                .First()
                .Version;

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());

            appBuilder.EnableApplicationInsights();

            appBuilder.UseWebApi(config);

        }
    }
}