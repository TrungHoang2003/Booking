using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Aggregates.AmenityAggregate;

public class AmenityGroup: Entity
{
    public string Name { get; private set; }
    public string IconUrl { get; private set; }
    public string Description { get; private set; }

    // Constructors
    public AmenityGroup(string name, string iconUrl, string description)
    {
        Name = name;
        IconUrl = iconUrl;
        Description = description;
    }

    public AmenityGroup(int id, string name, string iconUrl, string description) : base(id)
    {
        Name = name;
        IconUrl = iconUrl;
        Description = description;
    }
}