using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Entities;

public class Property: IAggregateRoot
{
    public Guid Id { get; set; }
    public Guid PropertyTypeId { get; set; }
    public Guid HostId { get; set; }
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan CheckOutTime { get; set; }
    public bool? PetAllowed { get; set; }
    public int AgeRestriction { get; set; }
    public int FloorNumber { get; set; }
    public string? Name { get; set; } 
}