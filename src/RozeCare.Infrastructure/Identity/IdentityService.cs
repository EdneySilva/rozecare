using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> CreateUserAsync(string email, string password, string name, string role, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            Name = name,
            Role = Enum.TryParse(role, out Domain.Enums.UserRole parsed) ? parsed : Domain.Enums.UserRole.Patient
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var message = string.Join(';', result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(message);
        }

        await _userManager.AddToRoleAsync(user, user.Role.ToString());
        return user;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
