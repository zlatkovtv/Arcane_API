using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// Middleware that catches all thrown exceptions and returns status code 500 as the response
public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        var exception = contextFeature.Error;
                        await context.Response.WriteAsync(new Error()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = exception.Message
                        }.ToString());
                    }
                });
            });
        }
    }