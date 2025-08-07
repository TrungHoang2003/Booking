using Contracts.DTOs;
using Contracts.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Api.Validators;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Api.Controllers;

[ApiController]
[Route("become-host-draft")]
public class BecomeHostDraftController(IBecomeHostDraftService service, IPublishEndpoint publishEndpoint) : Controller
{
    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        try
        {
            var draft = await service.StartAsync(userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to start draft: {ex.Message}");
        }
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get([FromQuery] Guid draftId)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        try
        {
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to get draft: {ex.Message}");
        }
    }

    [HttpPost("update-property-type")]
    public async Task<IActionResult> UpdatePropertyType([FromQuery] Guid draftId, [FromQuery] int propertyTypeId)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        try
        {
            await service.UpdatePropertyType(draftId, userIdInt, propertyTypeId);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update property type: {ex.Message}");
        }
    }

    [HttpPost("update-property-name")]
    public async Task<IActionResult> UpdatePropertyName([FromQuery] Guid draftId, [FromQuery] string propertyName)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");
        try
        {
            await service.UpdatePropertyName(draftId, userIdInt, propertyName);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update property name: {ex.Message}");
        }
    }

    [HttpPost("update-location")]
    public async Task<IActionResult> UpdateLocation([FromQuery] Guid draftId, [FromBody] LocationDto locationDto)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        if (locationDto == null)
            return BadRequest("Location information is required");

        try
        {
            await service.UpdateLocation(draftId, userIdInt, locationDto);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update location: {ex.Message}");
        }
    }

    [HttpPost("update-rental-unit")]
    public async Task<IActionResult> UpdateRentalUnit([FromQuery] Guid draftId, [FromBody] RentalUnitDto rentalUnitDto)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        if (rentalUnitDto == null)
            return BadRequest("Rental unit information is required");

        try
        {
            await service.UpdateRentalUnit(draftId, userIdInt, rentalUnitDto);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update rental unit: {ex.Message}");
        }
    }

    [HttpPost("update-bedroom")]
    public async Task<IActionResult> UpdateBedrooms([FromQuery] Guid draftId, [FromBody] List<BedroomDto> bedroomDtos)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        if (bedroomDtos == null || !bedroomDtos.Any())
            return BadRequest("At least one bedroom is required");

        try
        {
            await service.UpdateBedrooms(draftId, userIdInt, bedroomDtos);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update bedrooms: {ex.Message}");
        }
    }

    [HttpPost("update-amenities")]
    public async Task<IActionResult> UpdateAmenities([FromQuery] Guid draftId, [FromBody] List<int> amenities)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        if (amenities == null)
            return BadRequest("Amenities list cannot be null");

        try
        {
            await service.UpdateAmenities(draftId, userIdInt, amenities);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update amenities: {ex.Message}");
        }
    }

    [HttpPost("update-languages")]
    public async Task<IActionResult> UpdateLanguages([FromQuery] Guid draftId, [FromBody] List<int> languages)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        if (languages == null)
            return BadRequest("Languages list cannot be null");

        try
        {
            await service.UpdateLanguages(draftId, userIdInt, languages);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update languages: {ex.Message}");
        }
    }

    [HttpPost("update-houseRule")]
    public async Task<IActionResult> UpdateHouseRule([FromQuery] Guid draftId, [FromBody] HouseRuleDto houseRuleDto)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");
        if (houseRuleDto == null)
            return BadRequest("House rule information is required");

        try
        {
            await service.UpdateHouseRule(draftId, userIdInt, houseRuleDto);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update house rule: {ex.Message}");
        }
    }

    [HttpPost("update-photos")]
    public async Task<IActionResult> UpdatePhotos([FromQuery] Guid draftId, [FromBody] List<string> base64Images)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        if (base64Images == null)
            return BadRequest("Images list cannot be null");

        try
        {
            await service.UpdateImage(draftId, userIdInt, base64Images);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update photos: {ex.Message}");
        }
    }

    [HttpPost("update-Host-profile")]
    public async Task<IActionResult> UpdateHostProfile([FromQuery] Guid draftId, [FromBody] HostProfileDto hostProfileDto)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");
        
        try
        {
            await service.UpdateHostProfile(draftId, userIdInt, hostProfileDto);
            var draft = await service.GetAsync(draftId, userIdInt);
            return Ok(draft);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update photos: {ex.Message}");
        }
    }

    [HttpPost("submit-draft")]
    public async Task<IActionResult> CompleteDraft([FromQuery] Guid draftId)
    {
        var userId = Request.Headers["X-User-Id"].ToString();
        if (!int.TryParse(userId, out var userIdInt))
            return BadRequest("Invalid userId");

        try
        {
            var draft = await service.GetAsync(draftId, userIdInt);

            // Validate draft before submission
            var submissionValidation = BecomeHostDraftValidator.ValidateForSubmission(draft);
            if (!submissionValidation.IsValid)
                return BadRequest(submissionValidation.ErrorMessage);

            var startBecomeHost = new StartBecomeHost
            {
                CorrelationId = draftId,
                HostId = userIdInt,
                Draft = draft
            };
            
            await publishEndpoint.Publish(startBecomeHost);
            await service.CompleteDraft(draftId, userIdInt);

            return Ok(new
            {
                CorrelationId = draftId,
                Message = "Draft submitted successfully. Use the CorrelationId to track progress."
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to submit draft: {ex.Message}");
        }
    }
}