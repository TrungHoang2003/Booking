using BuildingBlocks.Commons;

namespace Property.Application.Errors;

public sealed record AmenityErrors
{
    public static readonly Error AmenityNotFound= new Error("Amenity Error", "Amenity not found");
}