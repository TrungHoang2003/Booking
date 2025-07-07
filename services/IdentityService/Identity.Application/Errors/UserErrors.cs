using BuildingBlocks.Commons;

namespace Identity.Application.Errors;

public sealed record UserErrors
{
   public static readonly Error UserNotFound = new Error("User Error", "User not found");
}