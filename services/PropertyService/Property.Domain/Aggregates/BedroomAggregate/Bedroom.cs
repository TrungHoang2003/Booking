using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.BedroomAggregate;

public class Bedroom:Entity
{
    public int RentalUnitId { get; private set; }
    public string Name { get; private set; }
    public bool SmokeAllowed { get; private set; }
    
    // Value Objects
    public BedType Type { get; private set; }

    // Constructors
    public Bedroom(int rentalUnitId, string name, bool smokeAllowed, BedType type)
    {
        RentalUnitId = rentalUnitId;
        Name = name;
        SmokeAllowed = smokeAllowed;
        Type = type;
    }
}