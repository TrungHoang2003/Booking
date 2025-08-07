using System.ComponentModel.DataAnnotations;
using Contracts.DTOs;

namespace Contracts.Drafts;

public class BecomeHostDraft
{
    [Required] public Guid DraftId { get; set; }
    [Required] public int HostId { get; set; }
    public int CurrentStep { get; set; }
    [Required] public int PropertyTypeId { get; set; }
    [Required] public string PropertyName { get; set; }
    [Required] public LocationDto LocationDto { get; set; }
    [Required] public RentalUnitDto RentalUnitDto { get; set; }
    [Required] public List<BedroomDto> ListBedroomDtos { get; set; }
    [Required] public List<int> AmenityIds { get; set; }
    [Required] public List<int> LanguageIds { get; set; }
    [Required] public HouseRuleDto HouseRuleDto { get; set; }
    [Required] public List<string> Base64Images { get; set; }
    [Required] public HostProfileDto HostProfileDto { get; set; }
}