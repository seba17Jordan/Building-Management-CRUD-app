using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuildingManagementApi.Filters
{
    public class AuthenticationFilter : Attribute, IActionFilter
    {
        private readonly IAuthenticationServiceLogic _userService;
        public List<Roles> Roles { get; set; }

        public AuthenticationFilter(Roles[] roles)
        {
            Roles = new List<Roles>(roles);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string header = context.HttpContext.Request.Headers["Authorization"];
            Guid token = Guid.Parse(header);
            if (header is null)
            {
                context.Result = new ObjectResult("Authorization header is required.")
                {
                    StatusCode = 401
                };
            }
            else {
                var role = _userService.GetUserRole(token);
                if (!Roles.Contains(role))
                {
                    context.Result = new ObjectResult("Unauthorized")
                    {
                        StatusCode = 403
                    };
                }
            }
        }

    }
}

