namespace Property.Domain.Entities;

public class Room
{
   public Guid Id { get; set; }
   public Guid PropertyId { get; set; }
   public int Size { get; set; } 
}