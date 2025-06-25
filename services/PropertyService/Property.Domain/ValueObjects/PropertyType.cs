namespace Property.Domain.ValueObjects;

public record PropertyType
{
   public string Value { get; init; }
   
   private PropertyType(string value)
   {
      if (string.IsNullOrEmpty(value))
         throw new ArgumentException("Property Type cannot be null or empty", nameof(value));
      Value = value;
   }
   
   public static PropertyType Apartment() => new("Apartment");
   public static PropertyType Homestay() => new("Homestay");
}