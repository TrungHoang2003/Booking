using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class EntirePropertyRentalUnit : RentalUnit
{
    public EntirePropertyRentalUnit(string? description, Price basePricePerNight, int maxAdults, int maxChildren, int size, int bedroomsCount, int bathroomsCount) : base(description, basePricePerNight, maxAdults, maxChildren)
    {
        Size = size;
        BedroomsCount = bedroomsCount;
        BathroomsCount = bathroomsCount;
    }
    
    public int Size { get; private set; } 
    public int BedroomsCount { get; private set; }
    public int BathroomsCount { get; private set; }
   
    public override RentalUnitType GetRentalUnitType()
    {
        return RentalUnitType.EntireProperty();
    }
}