using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BuberDinner.Api.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        // this filter applies to all of our controller when exception is thrown and not hanlded then this method will be called 
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            // use ProblemDetails to be more standard than errorResult
            var problemDetails = new ProblemDetails
            {
                Title = "An error occured while proccessing your request.",
                Status = (int)HttpStatusCode.InternalServerError

            };
            // var errorResult = new { error = "An error occured while proccessing your request." };

            context.Result = new ObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }
}
