using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IJwtService
{
    int GetUserIdFromToken(string? token);
    string? GenerateJwtToken(User user, string? role);
    string GenerateRefreshToken();
    int GetAccessTokenValidity();
    int GetRefreshTokenValidity();
}
