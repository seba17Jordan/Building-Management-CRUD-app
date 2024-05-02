using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CustomExceptions;
using System.Security.Authentication;

namespace BuildingManagementApi.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Argument exception. See: {context.Exception.Message}" })
                {
                    StatusCode = 400
                };
            }

            else if (context.Exception is ArgumentNullException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Argument null exception. See: {context.Exception.Message}" })
                {
                    StatusCode = 400
                };
            }
            else if (context.Exception is EmptyFieldException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Empty fields. See: {context.Exception.Message}"  })
                {
                    StatusCode = 400
                };
            }

            else if (context.Exception is ObjectAlreadyExistsException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Object already exists. See: {context.Exception.Message}" })
                {
                    StatusCode = 409
                };
            }

            else if (context.Exception is ObjectNotFoundException) {
                
                context.Result = new ObjectResult(new { ErrorMessage = $"Object not found. See: {context.Exception.Message}" })
                {
                    StatusCode = 404
                };
            }

            else if (context.Exception is InvalidOperationException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Invalid operation. See: {context.Exception.Message}" })
                {
                    StatusCode = 400
                };
            }

            else if (context.Exception is Exception)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Something went wrong. See: {context.Exception.Message}" })
                {
                    StatusCode = 500
                };
            }
        }    
    }
}
