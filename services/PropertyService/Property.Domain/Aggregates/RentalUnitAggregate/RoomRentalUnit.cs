using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class RoomRentalUnit : RentalUnit
{
    public RoomRentalUnit(int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity, bool useSharedBathroom) : base(propertyId, name, description, basePricePerNight, maxAdults, maxChildren, quantity)
    {
        UseSharedBathroom = useSharedBathroom;
    }

    public RoomRentalUnit(int id, int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity, bool useSharedBathroom) : base(id, propertyId, name, description, basePricePerNight, maxAdults, maxChildren, quantity)
    {
        UseSharedBathroom = useSharedBathroom;
    }

    public bool UseSharedBathroom { get; private set; }
   
    public override RentalUnitType GetRentalUnitType()
    {
        return RentalUnitType.Room();
    }
}