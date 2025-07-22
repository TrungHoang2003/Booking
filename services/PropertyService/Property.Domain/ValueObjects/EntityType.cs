namespace Property.Domain.ValueObjects;

public record EntityType
{
    public string Value { get; init;}
    
    public static EntityType Property => new("Property");

    private EntityType(string Value)
    {
        this.Value = Value;
    }
}