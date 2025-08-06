using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class EntirePropertyRentalUnit : RentalUnit
{
    protected EntirePropertyRentalUnit() {}
    
    public EntirePropertyRentalUnit(int maxAdults, int maxChildren, int size, int bedroomsCount, int bathroomsCount,  Price basePricePerNight, RentalUnitType rentalUnitType) : base( maxAdults, maxChildren, basePricePerNight, rentalUnitType)
    {
        Size = size;
        BedroomsCount = bedroomsCount;
        BathroomsCount = bathroomsCount;
    }
    
    public int Size { get; private set; } 
    public int BedroomsCount { get; private set; }
    public int BathroomsCount { get; private set; }
   
}