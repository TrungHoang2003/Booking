using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Aggregates.ImageAggregate;

public class Image: Entity
{
    public int EntityId { get; private set; }
    public string EntityType { get; private set; } 
    public string Url { get; private set; }
    public bool IsPrimary { get; private set; }
    
    // Constructors
    public Image(int entityId, string entityType, string url, bool isPrimary)
    {
        EntityId = entityId;
        EntityType = entityType;
        Url = url;
        IsPrimary = isPrimary;
    }
    public Image(int id, int entityId, string entityType, string url, bool isPrimary) : base(id)
    {
        EntityId = entityId;
        EntityType = entityType;
        Url = url;
        IsPrimary = isPrimary;
    }
}