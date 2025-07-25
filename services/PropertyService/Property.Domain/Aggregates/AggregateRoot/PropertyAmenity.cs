using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.AggregateRoot;

public class PropertyAmenity: Entity
{
    public int PropertyId { get; private set; }
    public int AmenityId { get; private set; }
    
    public string? Description { get; private set; }
   
    // Value Objects
    public Price? Price { get; private set; }
    
    // Constructors
    public PropertyAmenity(int propertyId, int amenityId, string? description)
    {
        if (propertyId <= 0) throw new ArgumentException("propertyId must be valid");
        if (amenityId <= 0) throw new ArgumentException("amenityId must be valid");
        
        PropertyId = propertyId;
        AmenityId = amenityId;
        Description = description;
    }
    
    public void SetPrice(Price price)
    {
        ArgumentNullException.ThrowIfNull(price);
        Price = price;
    }
}