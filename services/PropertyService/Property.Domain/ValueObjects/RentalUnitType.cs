namespace Property.Domain.ValueObjects;

public record RentalUnitType
{
    public string Value { get; init; }
    
    private RentalUnitType(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value cannot be null or empty");
        Value = value;
    }
    public static RentalUnitType Room() => new ("Room");
    public static RentalUnitType EntireProperty() => new("EntireProperty");

}