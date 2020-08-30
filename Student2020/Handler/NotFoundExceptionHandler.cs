using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Student2020.Handler
{
    public class NotFoundExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
           if(context.Exception is NotFoundException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = (int) HttpStatusCode.BadRequest
                };
            }
        }
    }
}
