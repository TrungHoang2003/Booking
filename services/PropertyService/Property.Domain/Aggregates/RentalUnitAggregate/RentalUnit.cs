using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.Aggregates.BedroomAggregate;
using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public abstract class RentalUnit: Entity
{
    public int PropertyId { get; set; }
    public int MaxAdults { get; set; }
    public int MaxChildren { get; set; }
   
    // Value Objects
    public Price BasePricePerNight { get; set; } 
    
    // Navigation properties
    public List<RentalUnitAmenity> Amenities = [];
    public List<Image> Images= [];
    public List<Bedroom> Bedrooms = [];
   
    // Constructors
    public RentalUnit(Price basePricePerNight, int maxAdults, int maxChildren) 
    {
        BasePricePerNight = basePricePerNight;
        MaxAdults = maxAdults;
        MaxChildren = maxChildren;
    }
    public abstract RentalUnitType GetRentalUnitType();
}