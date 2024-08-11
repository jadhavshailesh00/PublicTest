using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Interview.App_Start.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Log the exception
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            // Determine the status code based on the type of exception
            int statusCode = context.Exception switch
            {
                ArgumentNullException => 400,  // Bad Request
                UnauthorizedAccessException => 401,  // Unauthorized
                KeyNotFoundException => 404,  // Not Found
                _ => 500  // Internal Server Error (default)
            };

            // Set the result with the determined status code
            context.Result = new JsonResult(new
            {
                Error = context.Exception.Message, // Optionally include exception message
                ExceptionType = context.Exception.GetType().Name // Optionally include exception type
            })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }

}
