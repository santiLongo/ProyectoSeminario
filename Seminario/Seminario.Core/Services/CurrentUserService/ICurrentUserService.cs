namespace Seminario.Core.Services.CurrentUserService;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Name { get; }
    string? Role { get; }
}