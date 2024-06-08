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
            if (context.Exception is ArgumentNullException argNullException)
            {
                string customMessage = argNullException.ParamName;
                context.Result = new ObjectResult(new { ErrorMessage = $"{customMessage}" })
                {
                    StatusCode = 400
                };
            }

            else if (context.Exception is ArgumentException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"{context.Exception.Message}" })
                {
                    StatusCode = 400
                };
            }
           
            else if (context.Exception is EmptyFieldException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"{context.Exception.Message}"  })
                {
                    StatusCode = 400
                };
            }

            else if (context.Exception is ObjectAlreadyExistsException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"{context.Exception.Message}" })
                {
                    StatusCode = 409
                };
            }

            else if (context.Exception is ObjectNotFoundException) {
                
                context.Result = new ObjectResult(new { ErrorMessage = $"{context.Exception.Message}" })
                {
                    StatusCode = 404
                };
            }

            else if (context.Exception is InvalidOperationException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"{context.Exception.Message}" })
                {
                    StatusCode = 400
                };
            }

            else if (context.Exception is Exception)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"{context.Exception.Message}" })
                {
                    StatusCode = 500
                };
            }
        }    
    }
}
