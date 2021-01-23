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
                    var exceptionHandlingFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync($"{exceptionHandlingFeature?.Error.InnerException?.Message}");
                });
            });
        }
    }
}
