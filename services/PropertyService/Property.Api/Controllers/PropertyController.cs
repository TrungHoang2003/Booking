using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Property.Application.UseCases.Commands;

namespace Property.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PropertyController(IMediator mediator): Controller
{
   [HttpPost("CreateProperty")] 
   public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyCommand command)
   {
      var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
      
      if(!string.IsNullOrEmpty(userIdClaim?.Value) && int.TryParse(userIdClaim.Value, out var userId))
      {
         command = command with { HostId = userId };
         var result = await mediator.Send(command);
         return result.IsSuccess ? Ok(result) : BadRequest(result);
      }

      throw new UnauthorizedAccessException("User is not authenticated or userId is invalid.");
   }
}