using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Template.WebApi.Extensions
{
    public static class GlobalExceptionHandlerExtensions
    {
        public static void ConfigExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var response = new { Error = exception.Message };
                    await context.Response.WriteAsJsonAsync(response);
                });
            });
        }
    }
}
