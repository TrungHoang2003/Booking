using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public class EntirePropertyRentalUnit : RentalUnit
{
    public EntirePropertyRentalUnit(int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity, int size, int bedroomsCount, int bathroomsCount) : base(propertyId, name, description, basePricePerNight, maxAdults, maxChildren, quantity)
    {
        Size = size;
        BedroomsCount = bedroomsCount;
        BathroomsCount = bathroomsCount;
    }

    public EntirePropertyRentalUnit(int id, int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity, int size, int bedroomsCount, int bathroomsCount) : base(id, propertyId, name, description, basePricePerNight, maxAdults, maxChildren, quantity)
    {
        Size = size;
        BedroomsCount = bedroomsCount;
        BathroomsCount = bathroomsCount;
      
        if(quantity != 1) throw new ArgumentException("Entire property rental unit should have a quantity of 1.");
    }

    public int Size { get; private set; } 
    public int BedroomsCount { get; private set; }
    public int BathroomsCount { get; private set; }
   
    public override RentalUnitType GetRentalUnitType()
    {
        return RentalUnitType.EntireProperty();
    }
}