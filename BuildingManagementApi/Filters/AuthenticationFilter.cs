using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.Eventing.Reader;

namespace BuildingManagementApi.Filters
{
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        private readonly ISessionService _sessionService; //Ver si agrego al context o factory

        public AuthenticationFilter(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

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
            else
            {
                var currentUser = _sessionService.GetUserByToken(token);

                if (currentUser == null)
                {
                    context.Result = new ObjectResult(new { Message = "User not found" })
                    {
                        StatusCode = 403
                    };
                }
            }
        }
    }
}
