using Services.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Contracts
{
    public interface IJwtRepository
    {
        string GenerateJwtToken(LogedInUserViewModel loggedInPerson);
        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}