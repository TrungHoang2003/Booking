namespace Property.Domain.Entities;

public class Facility
{
   public Guid Id { get; set; }
   public Guid PropertyId { get; set; }
   public Guid FacilityTypeId { get; set; }
   public required string Name { get; set; } 
   public decimal? Fee { get; set; }
}