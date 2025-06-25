using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.Aggregates.AmenityAggregate;

namespace Property.Domain.Aggregates.AggregateRoot;

public class PropertyAmenity: Entity
{
    public int PropertyId { get; private set; }
    public int AmenityId { get; private set; }
    
    // Constructors
    public PropertyAmenity(int propertyId, int amenityId)
    {
        PropertyId = propertyId;
        AmenityId = amenityId;
    }
    public PropertyAmenity(int id, int propertyId, int amenityId) : base(id)
    {
        PropertyId = propertyId;
        AmenityId = amenityId;
    }
}