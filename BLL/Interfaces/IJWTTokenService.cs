using System.Security.Claims;

namespace BLL.Interfaces;

public interface IJWTTokenService
{
    public string GenerateToken(string email, string role);

    public ClaimsPrincipal? GetClaimsFromToken(string token);

    public string? GetClaimValue(string token, string claimType);
}
