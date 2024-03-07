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

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var jwtservice = context.HttpContext.RequestServices.GetService<IJwtRepository>();
            if (jwtservice == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "UnauthorizeUser" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "UnauthorizeUser" }));
                return;
            }

            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "UnauthorizeUser" }));
                return;
            }

            if (string.IsNullOrEmpty(_role) || roleClaim.Value != _role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "UnauthorizeUser" }));
            }


        }
    }

}
