using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class RoomRentalUnit : RentalUnit
{
    public RoomRentalUnit(string? name,  Price basePricePerNight, int maxAdults, int maxChildren, int quantity, bool sharedBathroom) : base(basePricePerNight, maxAdults, maxChildren)
    {
        SharedBathroom = sharedBathroom;
        Name = name;
        Quantity  = quantity;
        Type = RentalUnitType.Room();
    }
    
    public string? Name { get; private set; }
    public bool SharedBathroom { get; private set; }
    public int? Quantity { get; private set; }
}