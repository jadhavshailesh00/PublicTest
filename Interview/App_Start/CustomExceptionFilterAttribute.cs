using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Interview.App_Start
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<CustomExceptionFilterAttribute>>();
            var exception = context.Exception;
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case Exception notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    break;
               
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    logger?.LogError(exception, exception.Message);
                    break;
            }

            context.Result = new JsonResult(new { error = message })
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true; 
        }
    }

}
