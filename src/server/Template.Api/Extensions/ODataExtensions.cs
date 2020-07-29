using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Template.Domain.UsersModule;

namespace Template.Api.Extensions
{
    public static class ODataExtensions
    {
        public static void AddODataConfig(this IServiceCollection services)
        {
            services.AddOData();

            services.AddMvcCore(options =>
            {
                IEnumerable<ODataOutputFormatter> outputFormatters =
                    options.OutputFormatters.OfType<ODataOutputFormatter>()
                        .Where(foramtter => foramtter.SupportedMediaTypes.Count == 0);

                foreach (var outputFormatter in outputFormatters)
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/odata"));
                }
            });
        }

        public static void MapODataEndpoints(this IEndpointRouteBuilder endpoint)
        {
            endpoint.Select().Count().Filter().OrderBy().MaxTop(100);
            endpoint.MapODataRoute("ODataRoute", "odata", GetEdmModel());
        }

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            odataBuilder.EntitySet<User>("users");

            return odataBuilder.GetEdmModel();
        }
    }
}
