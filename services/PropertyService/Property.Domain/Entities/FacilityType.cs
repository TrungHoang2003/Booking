namespace Property.Domain.Entities;

public class FacilityType
{
   public Guid Id { get; set; }
   public required string Name { get; set; }
   public required string IconUrl { get; set; }
}