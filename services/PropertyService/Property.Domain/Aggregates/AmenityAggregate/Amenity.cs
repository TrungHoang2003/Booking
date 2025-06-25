using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.AmenityAggregate;

public class Amenity: Entity
{
   public int AmenityGroupId { get; private set; }
   public string Name { get; private set; }
   public string IconUrl { get; private set; }
   public string Description { get; private set; }
   public bool IsPaid { get; private set; }
   public Price Price { get; private set; }
   
   // Constructors
   public Amenity(int amenityGroupId, string name, string iconUrl, string description, bool isPaid, Price price)
   {
      AmenityGroupId = amenityGroupId;
      Name = name;
      IconUrl = iconUrl;
      Description = description;
      IsPaid = isPaid;
      Price = price;
   }
   
   public Amenity(int id, int amenityGroupId, string name, string iconUrl, string description, bool isPaid, Price price) : base(id)
   {
      AmenityGroupId = amenityGroupId;
      Name = name;
      IconUrl = iconUrl;
      Description = description;
      IsPaid = isPaid;
      Price = price;
   }
   
}