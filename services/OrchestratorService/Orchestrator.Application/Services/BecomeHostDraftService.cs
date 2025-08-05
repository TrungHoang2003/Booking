using System.Text.Json;
using BuildingBlocks.Interfaces;
using Contracts.Drafts;
using Contracts.DTOs;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Services;

public class BecomeHostDraftService(IRedisService redisService): IBecomeHostDraftService
{
    private readonly TimeSpan _ttl = TimeSpan.FromDays(7);
    
    public async Task<BecomeHostDraft> StartAsync(int userId)
    {
        var draft = new BecomeHostDraft
        {
            DraftId = Guid.NewGuid(),
            HostId = userId,
            CurrentStep = 1
        };
        await SaveAsync(draft);
        return draft;
    }

    public async Task<BecomeHostDraft> GetAsync(Guid draftId, int userId)
    {
        var json = await redisService.GetValue(Key(userId, draftId)) ?? throw new Exception("Draft json not found");
        return JsonSerializer.Deserialize<BecomeHostDraft>(json) ?? throw new Exception("Error while deserializing draft json");
    }

    public Task UpdatePropertyType(Guid draftId, int userid, int propertyTypeId)
    {
        return UpdateStep(draftId, userid, d =>
        {
            d.CurrentStep = 2;
            d.PropertyTypeId = propertyTypeId;
        });
    }
    
    public Task UpdatePropertyName(Guid draftId, int userid, string propertyName)
    {
        return UpdateStep(draftId, userid, d =>
        {
            d.CurrentStep = 3;
            d.PropertyName = propertyName;
        });
    }

    public Task UpdateLocation(Guid draftId, int userId, LocationDto locationDto)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.LocationDto = locationDto;
        });
    }

    public Task UpdateRentalUnit(Guid draftId, int userId, RentalUnitDto rentalUnitDto)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.RentalUnitDto = rentalUnitDto;
        });
    }

    public Task UpdateBedrooms(Guid draftId, int userId, List<BedroomDto> bedroomDtos)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.ListBedroomDtos= bedroomDtos;
        });
    }

    public Task UpdateAmenities(Guid draftId, int userId, List<int> amenityIds)
    {
        return UpdateStep(draftId, userId, draft =>
        {
            draft.AmenityIds = amenityIds;
        });
    }

    public Task UpdateHouseRule(Guid draftId, int userId, HouseRuleDto houseRuleDto)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.HouseRuleDto = houseRuleDto;
        });
    }

    public Task UpdateImage(Guid draftId, int userId, List<string> base64Images)
    {
        return UpdateStep(draftId, userId, d=>
        {
            d.Base64Images = base64Images;
        });
    }

    public Task UpdateHostProfile(Guid draftId, int userId, HostProfileDto hostProfileDto)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.HostProfileDto = hostProfileDto;
        });
    }

    public Task UpdateLanguages(Guid draftId, int userId, List<int> languageIds)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.LanguageIds = languageIds;
        });
    }

    private async Task SaveAsync(BecomeHostDraft draft)
    {
        await redisService.SetValue(Key(draft.HostId, draft.DraftId), JsonSerializer.Serialize(draft), _ttl);
    }

    private async Task UpdateStep(Guid draftId, int userId, Action<BecomeHostDraft> mutator)
    {
        var draft = await GetAsync(draftId, userId) ?? throw new Exception("Draft not found");
        mutator(draft);
        await SaveAsync(draft);
    }

    public async Task<bool> CompleteDraft(Guid draftId, int userId)
    {
        var draft = await GetAsync(draftId, userId) ?? throw new Exception("Draft not found");
        return await redisService.DeleteValue(Key(userId, draft.DraftId));
    }
    
    private static string Key(int userId, Guid id) 
        => $"draft:becomeHost:{userId}:{id}";
}