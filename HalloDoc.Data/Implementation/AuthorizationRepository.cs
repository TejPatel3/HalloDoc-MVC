using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Implementation
{
    public class AuthorizationRepository : Attribute, IAuthorizatoinRepository, IAuthorizationFilter
    {
        private readonly string _role;
        public AuthorizationRepository(string role = "")
        {
            _role = role;
        }
        private bool isAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtservice = context.HttpContext.RequestServices.GetService<IJwtRepository>();
            if (jwtservice == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminCredential", action = "AdminLogin" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken))
            {
                if (isAjaxRequest(request))
                {
                    context.Result = new JsonResult(new { error = "Failed to Authenticate User" })
                    {
                        StatusCode = 401
                    };
                }
                else
                {

                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = "AdminCredential",
                        Action = "AdminLogin"
                    }));
                }
                //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminCredential", action = "AdminLogin" }));
                return;
            }

            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminCredential", action = "AdminLogin" }));
                return;
            }

            if (string.IsNullOrEmpty(_role) || !_role.Contains(roleClaim.Value))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminCredential", action = "AdminLogin" }));
            }
        }
    }
}
