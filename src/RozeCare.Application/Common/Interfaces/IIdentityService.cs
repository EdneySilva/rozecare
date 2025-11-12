using RozeCare.Domain.Entities;

namespace RozeCare.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<ApplicationUser> CreateUserAsync(string email, string password, string name, string role, CancellationToken cancellationToken);

    Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
