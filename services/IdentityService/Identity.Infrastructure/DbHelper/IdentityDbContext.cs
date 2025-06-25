using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.DbHelper;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "Guest", NormalizedName = "GUEST" },
            new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" },
            new IdentityRole<int> { Id = 3, Name = "Host", NormalizedName = "HOST" }
        );
    } 
}