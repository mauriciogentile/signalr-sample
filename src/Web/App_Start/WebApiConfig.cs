using System.Net.Http.Formatting;
using System.Web.Http;

namespace Solaise.Weather.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.EnableSystemDiagnosticsTracing();
        }

        public static void RegisterFormatters(HttpConfiguration config)
        {
            config.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}
