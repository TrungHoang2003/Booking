using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class RoomRentalUnit : RentalUnit
{
    protected RoomRentalUnit(){}
    public RoomRentalUnit(string name, int maxAdults, int maxChildren, int? quantity, bool sharedBathroom, Price basePricePerNight, RentalUnitType rentalUnitType) : base( maxAdults, maxChildren, basePricePerNight, rentalUnitType)
    {
        SharedBathroom = sharedBathroom;
        Name = name;
        Quantity  = quantity;
    }
    
    public string Name { get; private set; }
    public bool SharedBathroom { get; private set; }
    public int? Quantity { get; private set; }
}