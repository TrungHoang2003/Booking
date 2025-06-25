namespace Property.Domain.ValueObjects;

public record BedType
{
   private string Value { get; init; }
   
   public static BedType Single() => new("Single");
   public static BedType Double() => new("Double");
   public static BedType Queen() => new("Queen");
   public static BedType King() => new("King");
   public static BedType SofaBed() => new("SofaBed");
   
   private BedType(string value)
   {
      if (string.IsNullOrEmpty(value))
         throw new ArgumentException("Bed Type cannot be null or empty", nameof(value));
      Value = value;
   }
}