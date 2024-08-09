using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Interview.App_Start
{
    public class CustomExceptionFilterAttribute : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            context.Result = new JsonResult(new
            {
                Error = "An internal error occurred. Please try again later. test"
            })
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
