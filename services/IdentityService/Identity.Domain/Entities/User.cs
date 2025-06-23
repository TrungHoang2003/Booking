using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User:IdentityUser<Guid>
{
    public string? FullName { get; set; }
}