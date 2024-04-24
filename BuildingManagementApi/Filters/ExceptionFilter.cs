using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BuildingManagementApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message })
                {
                    StatusCode = 404
                };
            }
            else if (context.Exception is Exception)
            {
                context.Result = new ObjectResult(new { ErrorMessage = $"Something went wrong. See: {context.Exception.Message}" })
                {
                    StatusCode = 500
                };
            }
            else if (context.Exception is InvalidOperationException) {
                context.Result = new ObjectResult(new { ErrorMessage = $"Something went wrong. See: {context.Exception.Message}" })
                {
                    StatusCode = 400
                };
            } 
        }    
    }
}
