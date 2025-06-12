using BuildingBlocks.ResultPattern;

namespace Identity.Domain.Errors;

public sealed record AuthenErrors
{
    public static readonly Error WrongPassword = new("Authen Errors", "Wrong password.");
    public static readonly Error UserNotFound = new("Authen Errors", "User not found.");
}