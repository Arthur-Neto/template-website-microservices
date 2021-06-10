using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Template.Application.UsersModule.Models;

namespace Template.WebApi.Extensions
{
    public static class ODataExtensions
    {
        public static void AddODataConfig(this IServiceCollection services)
        {
            services.AddOData(opt => opt
                .AddModel(
                    "odata",
                    GetEdmModel(),
                    builder => builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), sp => new StringAsEnumResolver())
                ).Count().Filter().Expand().Select().OrderBy().SetMaxTop(100)
            );
        }

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            odataBuilder.EntitySet<UserModel>("Users");

            return odataBuilder.GetEdmModel();
        }
    }
}
