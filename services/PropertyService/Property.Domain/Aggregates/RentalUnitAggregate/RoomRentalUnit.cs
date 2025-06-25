using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class RoomRentalUnit : RentalUnit
{
    public RoomRentalUnit(int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity, bool sharedBathroom) : base(propertyId, name, description, basePricePerNight, maxAdults, maxChildren, quantity)
    {
        SharedBathroom = sharedBathroom;
    }

    public RoomRentalUnit(int id, int propertyId, string name, string description, int maxAdults, int maxChildren, int quantity, bool sharedBathroom) : base(id, propertyId, name, description, maxAdults, maxChildren, quantity)
    {
        SharedBathroom = sharedBathroom;
    }

    public bool SharedBathroom { get; private set; }
   
    public override RentalUnitType GetRentalUnitType()
    {
        return RentalUnitType.Room();
    }
}