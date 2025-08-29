using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Repositories;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult> CreateAsync(IdentityRole<int> role);
}

public class RoleRepository(RoleManager<IdentityRole<int>> roleManager): IRoleRepository
{
    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IdentityResult> CreateAsync(IdentityRole<int> role)
    {
        return await roleManager.CreateAsync(role);
    }
    
}