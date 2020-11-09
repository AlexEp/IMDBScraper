using IMDBScraper.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDBScraper.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; 
            var code = 500; // Internal Server Error by default

            //if (exception is MyNotFoundException) code = 404; // Not Found
             if (exception is UnauthorizedAccessException) code = 401; // Unauthorized
            //else if (exception is MyException) code = 400; // Bad Request

            Response.StatusCode = code; // You can use HttpStatusCode enum instead

            return new ErrorResponse(exception); 
        }
    }
}
