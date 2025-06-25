using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class RentalUnitAmenity:Entity
{
    public int RentalUnitId { get; private set; }
    public int AmenityId { get; private set; }

    // Constructors
    public RentalUnitAmenity(int rentalUnitId, int amenityId)
    {
        RentalUnitId = rentalUnitId;
        AmenityId = amenityId;
    }

    public RentalUnitAmenity(int id, int rentalUnitId, int amenityId) : base(id)
    {
        RentalUnitId = rentalUnitId;
        AmenityId = amenityId;
    }
}