using Contracts.DTOs;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.DTOs;

namespace Orchestrator.Api.Interfaces;

public interface IBecomeHostDraftService
{
   Task<BecomeHostDraft> StartAsync(int userId);
   Task<BecomeHostDraft?> GetAsync(Guid draftId, int userId);
   Task UpdatePropertyType(Guid draftId, int userid, int propertyTypeId);
   Task UpdatePropertyName(Guid draftId, int userid, string propertyName);
   Task UpdateLocation(Guid draftId, int userId, LocationDto locationDto);
   Task UpdateRentalUnit(Guid draftId, int userId, RentalUnitDto rentalUnitDto);
   Task UddateAmenities(Guid draftId, int userId, List<int> amenities);
   Task UpdateHouseRule(Guid draftId, int userId, HouseRuleDto houseRuleDto);
   Task UpdateImage(Guid draftId, int userId, List<ImageDto> images);
   Task UpdatePricePerNight(Guid draftId, int userId, decimal pricePerNight);
   Task<bool> CompleteDraft(Guid draftId, int userId);
}