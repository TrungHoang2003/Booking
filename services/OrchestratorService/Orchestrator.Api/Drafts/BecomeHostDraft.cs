using Contracts.DTOs;
using Orchestrator.Api.DTOs;

namespace Orchestrator.Api.Drafts;

public class BecomeHostDraft
{
    public Guid DraftId { get; set; }
    public int HostId { get; set; }
    public int CurrentStep { get; set; }
    public int PropertyTypeId { get; set; } 
    public string PropertyName { get; set; }
    public LocationDto LocationDto { get; set; }
    public RentalUnitDto RentalUnitDto { get; set; }
    public List<int> AmenityIds { get; set; } 
    public HouseRuleDto HouseRuleDto { get; set; }
    public List<ImageDto> Photos { get; set; } 
    public decimal PricePerNight { get; set; }
}