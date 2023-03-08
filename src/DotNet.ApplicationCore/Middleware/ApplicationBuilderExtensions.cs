using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
}
