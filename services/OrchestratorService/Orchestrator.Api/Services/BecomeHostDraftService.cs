using System.Text.Json;
using BuildingBlocks.Interfaces;
using Contracts.DTOs;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.DTOs;
using Orchestrator.Api.Interfaces;

namespace Orchestrator.Api.Services;

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

    public Task UpdateImage(Guid draftId, int userId, List<ImageDto> images)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.Photos = images;
        });
    }

    public Task UpdatePricePerNight(Guid draftId, int userId, decimal pricePerNight)
    {
        return UpdateStep(draftId, userId, d =>
        {
            d.PricePerNight = pricePerNight;
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