using BuildingBlocks.Interfaces;

namespace Contracts.Messages;

public class CreateProperty: IMessage
{
    public Guid CorrelationId { get; set; }
    public int HostId { get; set; }
    public string Name { get; set; }
    public int PropertyTypeId { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int PostCode { get; set; }
    public TimeSpan CheckInTimeFrom { get; set; }
    public TimeSpan CheckInTimeUntil { get; set; }
    public TimeSpan CheckOutTimeFrom { get; set; }
    public TimeSpan CheckOutTimeUntil { get; set; }
    public bool PetAllowed { get; set; }
    public bool SmokingAllowed { get; set; }
    public bool PartyAllowed { get; set; }
    public int AgeRestriction { get; set; }
    public int FloorNumber { get; set; }
    public List<int>? LanguageIds { get; set; }
    public List<string>? Base64Images { get; set; }
    public string? Description { get; set; }
    public string? NeighborhoodDescription { get; set; }
}