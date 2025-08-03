using Contracts.DTOs;
using Contracts.Events;
using Contracts.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.Interfaces;

namespace Orchestrator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BecomeHostDraftController(IBecomeHostDraftService service, IPublishEndpoint publishEndpoint) : Controller
{
    [HttpPost("start")]
    public async Task<BecomeHostDraft> Start()
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        return await service.StartAsync(userIdInt);
    }
    
    [HttpPost("get")]
    public async Task<BecomeHostDraft> Get([FromBody] Guid draftId )
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        var draft = await service.GetAsync(draftId, userIdInt);
        return draft;
    }

    [HttpPost("update-property-type")]
    public async Task UpdatePropertyType([FromQuery] Guid draftId, [FromQuery] int propertyTypeId)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdatePropertyType(draftId, userIdInt, propertyTypeId);
    }
    
    [HttpPost("update-property-name")]
    public async Task UpdatePropertyName([FromQuery] Guid draftId, [FromQuery] string propertyName)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdatePropertyName(draftId, userIdInt, propertyName);
    }
    
    [HttpPost("update-location")]
    public async Task UpdateLocation([FromQuery] Guid draftId, [FromBody] LocationDto locationDto)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateLocation(draftId, userIdInt, locationDto);
    }
    
    [HttpPost("update-rental-unit")]
    public async Task UpdateRentalUnit([FromQuery] Guid draftId, [FromBody] RentalUnitDto rentalUnitDto)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateRentalUnit(draftId, userIdInt, rentalUnitDto);
    }

    [HttpPost("update-bedroom")]
    public async Task UpdateBedrooms([FromQuery] Guid draftId, [FromBody] List<BedroomDto> bedroomDtos)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateBedrooms(draftId, userIdInt, bedroomDtos); 
    }
    
    [HttpPost("update-amenities")]
    public async Task UpdateAmenities([FromQuery] Guid draftId, [FromBody] List<int> amenities)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateAmenities(draftId, userIdInt, amenities);
    }
    
    [HttpPost("update-languages")]
    public async Task UpdateLanguages([FromQuery] Guid draftId, [FromBody] List<int> languages)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateLanguages(draftId, userIdInt, languages);
    }
    
    [HttpPost("update-houseRule")]
    public async Task UpdateHouseRule([FromQuery] Guid draftId, [FromBody] HouseRuleDto houseRuleDto)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateHouseRule(draftId, userIdInt, houseRuleDto);
    }
    
    [HttpPost("update-photos")]
    public async Task UpdatePhotos([FromQuery] Guid draftId, [FromBody] List<string> base64Images)
    {
        var userId = Request.Headers["X-User-Id"];
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        await service.UpdateImage(draftId, userIdInt, base64Images);
    }
    
    [HttpPost("submit-draft")]
    public async Task CompleteDraft([FromQuery] Guid draftId)
    {
        var userId = Request.Headers["X-User-Id"];

        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
        }
        
        var draft = await service.GetAsync(draftId, userIdInt);
        var correlationId = Guid.NewGuid();
       
        // public Become Host Saga 
        var startSagaEvent = new BecomeHostStarted(correlationId, userIdInt);

        await publishEndpoint.Publish(startSagaEvent);
        await service.CompleteDraft(draftId, userIdInt);
    }
    
}