using BuildingBlocks.Commons;

namespace Property.Application.Errors;

public sealed record PropertyErrors
{
   public static readonly Error PropertyNotFound = new Error("Property Error", "Property not found");
}