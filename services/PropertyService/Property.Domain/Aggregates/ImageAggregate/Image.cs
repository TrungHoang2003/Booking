using BuildingBlocks.Interfaces;
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
    public Image(int entityId, string url, bool isPrimary)
    {
        EntityId = entityId;
        Url = url;
        IsPrimary = isPrimary;
    }
    
    public void SetEntityType(EntityType entityType)
    {
        EntityType = entityType;
    }

    public void SetPrimary()
    {
        IsPrimary = true;
    }
}
