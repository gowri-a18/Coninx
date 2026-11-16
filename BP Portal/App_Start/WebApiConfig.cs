using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace DemoApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.EnableSystemDiagnosticsTracing();
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            // Adding JSON type web api formatting.  
            config.Formatters.Clear();
            config.Formatters.Add(formatter);
            // Adding formatter for Json   
            ////config.Formatters.JsonFormatter.MediaTypeMappings.Add(
            ////    new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));
            ////// configure additional webapi settings here..
        }
    }
}