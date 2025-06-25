using BuildingBlocks.DomainDrivenPattern;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.RentalUnitAggregate;

public abstract class RentalUnit: Entity
{
   public int PropertyId { get; set; }
   public required string Name { get; set; } 
   public required string Description { get; set; }
   public Price BasePricePerNight { get; set; } 
   public int MaxAdults { get; set; }
   public int MaxChildren { get; set; }
   public int Quantity { get; set; }

   protected RentalUnit(int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity) 
   {
      PropertyId = propertyId;
      Name = name;
      Description = description;
      BasePricePerNight = basePricePerNight;
      MaxAdults = maxAdults;
      MaxChildren = maxChildren;
      Quantity = quantity;
   }

   protected RentalUnit(int id, int propertyId, string name, string description, Price basePricePerNight, int maxAdults, int maxChildren, int quantity):base(id)
   {
      PropertyId = propertyId;
      Name = name;
      Description = description;
      BasePricePerNight = basePricePerNight;
      MaxAdults = maxAdults;
      MaxChildren = maxChildren;
      Quantity = quantity;
   }
   
   public abstract RentalUnitType GetRentalUnitType();
}