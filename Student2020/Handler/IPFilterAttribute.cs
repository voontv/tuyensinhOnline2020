using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Student2020.Handler
{
    public class IPFilterAttribute: ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _accessor;

        public IPFilterAttribute(IHttpContextAccessor contextAccessor)
        {
            this._accessor = contextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var fileName = "ip_config.txt";

            if (!File.Exists(fileName))
            {
                context.Result = context.Result = new ObjectResult("Chưa config IP")
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            else
            {
                var connection = _accessor.HttpContext.Connection;

                var ip = connection.RemoteIpAddress.ToString();

                var allowedIps = File.ReadAllLines(fileName);

                if (!allowedIps.Contains(ip))
                {
                    context.Result = context.Result = new ObjectResult($"Địa chỉ IP {ip} không được cấu hình để truy cập tài nguyên này.")
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}
