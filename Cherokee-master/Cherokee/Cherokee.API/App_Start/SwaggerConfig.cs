using System.Web.Http;
using WebActivatorEx;
using Cherokee.API;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Cherokee.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Cherokee.API");
                    c.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}\\bin\\Cherokee.API.XML");
                })
                .EnableSwaggerUi();
        }
    }
}
