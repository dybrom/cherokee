using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Cherokee.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.EnableCors(new EnableCorsAttribute("*", "*", "*", "Pagination"));

            // Web API routes

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //   name: "CalendarAPI",
            //   routeTemplate: "api/employees/{id}/calendar/{year}/{month}",
            //   defaults: new { controller = "calendar", year = RouteParameter.Optional, month = RouteParameter.Optional }
            //);

            // config.Routes.MapHttpRoute(
            // name: "AnnualReport",
            // routeTemplate: "api/reports/{controller}/{year}",
            // defaults: new { year = RouteParameter.Optional, month = RouteParameter.Optional }
            // );

            // config.Routes.MapHttpRoute(
            //name: "MonthlyReport",
            //routeTemplate: "api/reports/{controller}/{year}/{month}",
            //defaults: new { year = RouteParameter.Optional, month = RouteParameter.Optional }
            //);

            // config.Routes.MapHttpRoute(
            // name: "PersonalReprt",
            // routeTemplate: "api/reports/personal/{employeeId}/{year}/{month}",
            // defaults: new { year = RouteParameter.Optional, month = RouteParameter.Optional, employeeId = RouteParameter.Optional }
            // );



            // config.Routes.MapHttpRoute(
            //    name: "PagingAPI",
            //    routeTemplate: "api/{controller}/{page}",
            //    defaults: new {  page = RouteParameter.Optional }
            //);


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );




            var json = GlobalConfiguration.Configuration;
            json.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            json.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
           // json.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
           // json.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            json.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            /*
             * 
             *  Kada unosimo put onda foreign key ide  kao objekat { id : "", name: ""}
             * 
             * */

        }
    }
}
