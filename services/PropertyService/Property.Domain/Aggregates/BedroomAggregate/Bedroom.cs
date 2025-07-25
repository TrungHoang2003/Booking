using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.BedroomAggregate;

public class Bedroom:Entity
{
    public int RentalUnitId { get; private set; }
    public string? Name { get; private set; }
    public int Quantity { get; private set; }
    public int SingleBeds { get; private set; } = 0;
    public int DoubleBeds { get; private set; } = 0;
    public int KingBeds { get; private set; } = 0;
    public int SofaBeds { get; private set; } = 0;
    public bool SmokeAllowed { get; private set; }
    
    // Constructors
    public Bedroom(int rentalUnitId, string? name, bool smokeAllowed, int quantity, int singleBeds, int doubleBeds, int kingBeds, int sofaBeds)
    {
        RentalUnitId = rentalUnitId;
        Name = name;
        SmokeAllowed = smokeAllowed;
        Quantity = quantity;
        SingleBeds = singleBeds;
        DoubleBeds = doubleBeds;
        KingBeds = kingBeds;
        SofaBeds = sofaBeds;
    }
    
}