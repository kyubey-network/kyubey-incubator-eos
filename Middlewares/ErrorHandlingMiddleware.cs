using Andoromeda.Framework.Logger;
using Andoromeda.Kyubey.Incubator.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Andoromeda.Kyubey.Incubator.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment env)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IHostingEnvironment env)
        {
            var exceptionMessage = exception.ToString();

            var code = HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new ApiResult
            {
                code = 500,
                msg = env.IsDevelopment() ? exceptionMessage : null
            });

            if (!env.IsDevelopment())
            {
                var logger = (ILogger)context.RequestServices.GetService(typeof(ILogger));
                logger.LogError(exceptionMessage);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
