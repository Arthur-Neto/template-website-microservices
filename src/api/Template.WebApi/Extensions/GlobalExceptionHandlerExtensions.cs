using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Template.Infra.Crosscutting.Exceptions;

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

                    if (exceptionHandlingFeature?.Error is GuardException ex)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync($"{{ \"error\": \"{ex.Message}\" }}");
                    }
                    else
                    {
                        await context.Response.WriteAsync($"{exceptionHandlingFeature?.Error.InnerException.Message}");
                    }
                });
            });
        }
    }
}
