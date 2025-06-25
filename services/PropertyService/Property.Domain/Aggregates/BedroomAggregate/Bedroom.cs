using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.BedroomAggregate;

public class Bedroom:Entity
{
    public int RentalUnitId { get; private set; }
    public string Name { get; private set; }
    public string CotQuantity { get; private set; }
    public bool SmokeAllowed { get; private set; }
    
    // Value Objects
    public Price CotPrice { get; private set; }
    public BedType Type { get; private set; }

    // Constructors
    public Bedroom(int rentalUnitId, string name, string cotQuantity, bool smokeAllowed, BedType type, Price cotPrice)
    {
        RentalUnitId = rentalUnitId;
        Name = name;
        CotQuantity = cotQuantity;
        SmokeAllowed = smokeAllowed;
        Type = type;
        CotPrice = cotPrice;
    }

    private Bedroom(int id, int rentalUnitId, string name, string cotQuantity, bool smokeAllowed) : base(id)
    {
        RentalUnitId = rentalUnitId;
        Name = name;
        CotQuantity = cotQuantity;
        SmokeAllowed = smokeAllowed;
    }
}