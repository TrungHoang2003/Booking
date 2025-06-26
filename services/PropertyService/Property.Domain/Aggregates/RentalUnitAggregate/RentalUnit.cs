using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.Aggregates.BedroomAggregate;
using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public abstract class RentalUnit: Entity
{
    public int PropertyId { get; set; }
    public string Name { get; set; } 
    public string? Description { get; set; }
    public int MaxAdults { get; set; }
    public int MaxChildren { get; set; }
    public int Quantity { get; set; }
   
    // Value Objects
    public Price BasePricePerNight { get; set; } 
    
    // Navigation properties
    public List<RentalUnitAmenity> Amenities = [];
    public List<Image> Images= [];
    public List<Bedroom> Bedrooms = [];
   
    // Constructors
    public RentalUnit(string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity) 
    {
        Name = name;
        Description = description;
        BasePricePerNight = basePricePerNight;
        MaxAdults = maxAdults;
        MaxChildren = maxChildren;
        Quantity = quantity;
    }
   
    public abstract RentalUnitType GetRentalUnitType();
}