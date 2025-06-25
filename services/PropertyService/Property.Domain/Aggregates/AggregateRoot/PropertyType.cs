using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Aggregates.AggregateRoot;

public class PropertyType:Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string IconUrl { get; private set; }
    
    // Constructors
    public PropertyType(string name, string description, string iconUrl)
    {
        Name = name;
        Description = description;
        IconUrl = iconUrl;
    }
    public PropertyType(int id, string name, string description, string iconUrl) : base(id)
    {
        Name = name;
        Description = description;
        IconUrl = iconUrl;
    }
}