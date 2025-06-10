using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User:IdentityUser<int>
{
    public string ? FullName { get; set; }
}