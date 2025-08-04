using BuildingBlocks.Interfaces;

namespace Property.Domain.Aggregates.AggregateRoot;

public class PropertyType:Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsRoomBased { get; private set; }
    
    // Constructors
    public PropertyType(string name, string description, bool isRoomBased)
    {
        Name = name;
        Description = description;
        IsRoomBased = isRoomBased;
    }
    public PropertyType(int id, string name, string description, bool isRoomBased) : base(id)
    {
        Name = name;
        Description = description;
        IsRoomBased = isRoomBased;
    }
}