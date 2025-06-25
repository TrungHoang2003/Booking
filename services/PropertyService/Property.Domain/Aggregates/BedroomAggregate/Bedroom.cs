using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.BedroomAggregate;

public class Bedroom:Entity
{
    public int RentalUnitId { get; private set; }
    public string Name { get; private set; }
    public string CotQuantity { get; private set; }
    public Price CotPrice { get; private set; }
    public bool SmokeAllowed { get; private set; }
    
    // Value Objects
    public BedType Type { get; private set; }

    // Constructors
    public Bedroom(int rentalUnitId, string name, string cotQuantity, Price cotPrice, bool smokeAllowed, BedType type)
    {
        RentalUnitId = rentalUnitId;
        Name = name;
        CotQuantity = cotQuantity;
        CotPrice = cotPrice;
        SmokeAllowed = smokeAllowed;
        Type = type;
    }
    public Bedroom(int id, int rentalUnitId, string name, string cotQuantity, Price cotPrice, bool smokeAllowed, BedType type) : base(id)
    {
        RentalUnitId = rentalUnitId;
        Name = name;
        CotQuantity = cotQuantity;
        CotPrice = cotPrice;
        SmokeAllowed = smokeAllowed;
        Type = type;
    }
}