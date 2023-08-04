using BtkAkademi.Entities.ErrorModels;
using BtkAkademi.Entities.Exceptions;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace BtkAkademi.WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app,
            ILoggerService _logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (context is not null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        _logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetail()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });

            });
        }
    }
}
