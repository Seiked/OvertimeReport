using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Api.Models;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleUnauthorizedAsync(httpContext);
                }
                else if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await HandleForbiddenAsync(httpContext);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            var problem = new CustomValidationProblemDetails
            {
                Success = false,
                Errors =
                [
                    ex.Message,
                    ex.StackTrace!
                ],
                StatusCode = HttpStatusCode.InternalServerError,
            };
            await httpContext.Response.WriteAsJsonAsync(problem);
        }

        private Task HandleUnauthorizedAsync(HttpContext context)
        {
            var problem = new CustomValidationProblemDetails
            {
                Success = false,
                Errors =
                [
                    "No estas autorizado para entrar. Porfavor inicia sesion e intenta  otravez.",
                ],
                StatusCode = HttpStatusCode.Unauthorized,
            };
            return context.Response.WriteAsJsonAsync(problem);
        }

        private Task HandleForbiddenAsync(HttpContext context)
        {
            var problem = new CustomValidationProblemDetails
            {
                Success = false,
                Errors =
              [
                 "No tienes permisos para entrar. Si necesitas acceso, porfavor contacta a un administrador.",
              ],
                StatusCode = HttpStatusCode.Forbidden,
            };
            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}