using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.ImageAggregate;

public class Image: Entity
{
    public int EntityId { get; private set; }
    public string Url { get; private set; }
    public bool IsPrimary { get; private set; }
    
    // Value Objects
    public EntityType EntityType { get; private set; } 
    
    // Constructors
    public Image(int entityId, EntityType type, string url, bool isPrimary)
    {
        EntityId = entityId;
        EntityType = type;
        Url = url;
        IsPrimary = isPrimary;
    }

    public void SetPrimary()
    {
        IsPrimary = true;
    }
}
