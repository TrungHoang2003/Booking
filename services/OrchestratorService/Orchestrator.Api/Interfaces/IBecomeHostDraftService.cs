using Contracts.DTOs;
using Contracts.Messages;
using Orchestrator.Api.Drafts;

namespace Orchestrator.Api.Interfaces;

public interface IBecomeHostDraftService
{
   Task<BecomeHostDraft> StartAsync(int userId);
   Task<BecomeHostDraft> GetAsync(Guid draftId, int userId);
   Task UpdatePropertyType(Guid draftId, int userid, int propertyTypeId);
   Task UpdatePropertyName(Guid draftId, int userid, string propertyName);
   Task UpdateLocation(Guid draftId, int userId, LocationDto locationDto);
   Task UpdateRentalUnit(Guid draftId, int userId, RentalUnitDto rentalUnitDto);
   Task UpdateAmenities(Guid draftId, int userId, List<int> amenities);
   Task UpdateLanguages(Guid draftId, int userId, List<int> languageIds);
   Task UpdateHouseRule(Guid draftId, int userId, HouseRuleDto houseRuleDto);
   Task UpdateImage(Guid draftId, int userId, List<string> base64Images);
   Task<bool> CompleteDraft(Guid draftId, int userId);
}