using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Student2020.Handler
{
    public class BadRequestExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BadRequestException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = (int) HttpStatusCode.BadRequest
                };
            }
        }
    }
}
