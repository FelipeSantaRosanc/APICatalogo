using System.Net;
using APICatalogo.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace APICatalogo.Extensions
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            //middleware para tratamento de exceção
            app.UseExceptionHandler(appError =>
            {
                //tratamento de exceção global
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //informações de erro no tratamento de exceções
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }



    }
}
