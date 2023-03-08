using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using DotNet.ApplicationCore.Exceptions;

namespace DotNet.ApplicationCore.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            string message;


            var exceptionType = exception.GetType();

            if (exceptionType == typeof(BadRequestException))
            {
                message = exception.Message;
                status = HttpStatusCode.BadRequest;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                status = HttpStatusCode.NotFound;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(HadReferanceException))
            {
                status = HttpStatusCode.Locked;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message + " (" + exception.InnerException.ToString() + ")";
                stackTrace = exception.StackTrace;
            }

            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace, status });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
