namespace Property.Domain.ValueObjects;

public record RentalUnitType
{
    public string Value { get; init; }
    
    private RentalUnitType(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Data cannot be null or empty");
        Value = value;
    }
    public static RentalUnitType Room() => new ("Room");
    public static RentalUnitType EntireProperty() => new("EntireProperty");
    
    public static RentalUnitType FromValue(string value) => value switch
    {
        "RoomBased" => Room(),
        "EntireProperty" => EntireProperty(),
        _ => throw new ArgumentException($"Invalid RentalUnitType value: {value}")
    };

}