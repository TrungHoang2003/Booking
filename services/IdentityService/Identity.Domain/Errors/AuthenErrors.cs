using BuildingBlocks.ResultPattern;

namespace Identity.Domain.Errors;

public sealed record AuthenErrors
{
public static readonly Error WrongPassword= new Error("Authen Erros", "Wrong password.");
   public static readonly Error UserNotFound = new Error("Authen Erros", "User not found.");
}