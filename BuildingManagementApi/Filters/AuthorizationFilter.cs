using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.Eventing.Reader;

namespace BuildingManagementApi.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public Roles _currentRole { get; set; } 

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Desde aca mando el token a quien lo va a usar
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            Guid token = Guid.Empty;

            if (string.IsNullOrEmpty(authHeader) || !Guid.TryParse(authHeader, out token))
            {
                context.Result = new ObjectResult(new { Message = "No authorization header" })
                {
                    StatusCode = 401
                };
            }
            
            var _sessionService = (ISessionService)context.HttpContext.RequestServices.GetService(typeof(ISessionService));
            var currentUser = _sessionService.GetUserByToken(token);

            if (currentUser == null)
            {
                context.Result = new ObjectResult(new { Message = "User not found" })
                {
                    StatusCode = 403
                };
            }

            if (currentUser.Role != _currentRole)
            {
                context.Result = new ObjectResult(new { Message = "User not authorized" })
                {
                    StatusCode = 403
                };
            }
            
        }
    }
}

