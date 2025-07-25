using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class EntirePropertyRentalUnit : RentalUnit
{
    public EntirePropertyRentalUnit(int maxAdults, int maxChildren, int size, int bedroomsCount, int bathroomsCount) : base( maxAdults, maxChildren)
    {
        Size = size;
        BedroomsCount = bedroomsCount;
        BathroomsCount = bathroomsCount;
        Type = RentalUnitType.EntireProperty();
    }
    
    public int Size { get; private set; } 
    public int BedroomsCount { get; private set; }
    public int BathroomsCount { get; private set; }
   
}