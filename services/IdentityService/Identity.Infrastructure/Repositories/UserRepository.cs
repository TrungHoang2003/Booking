using Identity.Domain.Entities;
using Identity.Infrastructure.DbHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Repositories;

public interface IUserRepository 
{
    Task<User?> GetById(int id);
    Task<User?> FindByNameAsync(string userName);
    Task<IList<string>> GetRolesAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IdentityResult> CreateAsync(User user, string? password = null);
    Task<User?> FindByEmailAsync(string email);
    Task<IdentityResult> AddToRoleAsync(User user, string role);
}

public class UserRepository(UserManager<User> userManager, IdentityDbContext dbContext, ILogger<UserRepository> logger) :  IUserRepository
{
    public async Task<User?> GetById(int id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    public async Task<User?> FindByNameAsync(string userName)
    {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateAsync(User user, string? password = null)
    {
        return password == null
            ? await userManager.CreateAsync(user)
            : await userManager.CreateAsync(user, password);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        try
        {
            return await userManager.GetRolesAsync(user);
        }
        catch (Exception e)
        {
            logger.LogError("Error while getting Roles: {e.Message}", e.Message);
            throw;
        }
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        return await userManager.AddToRoleAsync(user, role);
    }
}
