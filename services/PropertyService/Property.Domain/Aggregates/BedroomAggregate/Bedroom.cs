using BuildingBlocks.Interfaces;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.BedroomAggregate;

public class Bedroom:Entity
{
    public int RentalUnitId { get; private set; }
    public int DoubleBeds { get; private set; } = 0;
    public int SingleBeds { get; private set; } = 0;
    public int KingBeds { get; private set; } = 0;
    public int SofaBeds { get; private set; } = 0;
    
    // Constructors
    public Bedroom(int rentalUnitId, int singleBeds, int doubleBeds, int kingBeds, int sofaBeds)
    {
        RentalUnitId = rentalUnitId;
        SingleBeds = singleBeds;
        DoubleBeds = doubleBeds;
        KingBeds = kingBeds;
        SofaBeds = sofaBeds;
    }
    
}