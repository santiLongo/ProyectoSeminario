using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Seminario.Datos.Services.CurrentUserService;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Name { get; }
    string? Role { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId =>
        int.TryParse(
            _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var id
        ) ? id : null;

    public string? Name =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirst(JwtRegisteredClaimNames.Name)?.Value;

    public string? Role =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.Role)?.Value;
}