using Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.DTOs;
using Orchestrator.Api.Interfaces;

namespace Orchestrator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BecomeHostDraftController(IBecomeHostDraftService service) : Controller
{
    [HttpPost("Start")]
    public async Task<BecomeHostDraft> Start()
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        return await service.StartAsync(userIdInt);
    }
    
    [HttpPost("Get")]
    public async Task<BecomeHostDraft?> Get([FromBody] Guid draftId )
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        var draft = await service.GetAsync(draftId, userIdInt);
        return draft;
    }

    [HttpPost("UpdatePropertyType")]
    public async Task UpdatePropertyType([FromBody] Guid draftId, [FromQuery] int propertyTypeId)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdatePropertyType(draftId, userIdInt, propertyTypeId);
    }
    
    [HttpPost("UpdatePropertyName")]
    public async Task UpdatePropertyName([FromBody] Guid draftId, [FromQuery] string propertyName)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdatePropertyName(draftId, userIdInt, propertyName);
    }
    
    [HttpPost("UpdateLocation")]
    public async Task UpdateLocation([FromBody] Guid draftId, [FromBody] LocationDto locationDto)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateLocation(draftId, userIdInt, locationDto);
    }
    
    [HttpPost("UpdateRentalUnit")]
    public async Task UpdateRentalUnit([FromBody] Guid draftId, [FromBody] RentalUnitDto rentalUnitDto)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateRentalUnit(draftId, userIdInt, rentalUnitDto);
    }
    
    [HttpPost("UpdateAmenities")]
    public async Task UpdateAmenities([FromBody] Guid draftId, [FromBody] List<int> amenities)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateAmenities(draftId, userIdInt, amenities);
    }
    
    [HttpPost("UpdateHouseRule")]
    public async Task UpdateHouseRule([FromBody] Guid draftId, [FromBody] HouseRuleDto houseRuleDto)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateHouseRule(draftId, userIdInt, houseRuleDto);
    }
    
    [HttpPost("UpdatePhotos")]
    public async Task UpdatePhotos([FromBody] Guid draftId, [FromBody] List<ImageDto> images)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateImage(draftId, userIdInt, images);
    }
    
    [HttpPost("UpdatePricePerNight")]
    public async Task UpdatePricePerNight([FromBody] Guid draftId, [FromQuery] decimal pricePerNight)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdatePricePerNight(draftId, userIdInt, pricePerNight);
    }
    
    [HttpPost("CompleteDraft")]
    public async Task CompleteDraft([FromBody] Guid draftId)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.CompleteDraft(draftId, userIdInt);
    }
    
}