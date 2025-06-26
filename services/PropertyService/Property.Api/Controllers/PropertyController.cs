using MediatR;
using Microsoft.AspNetCore.Mvc;
using Property.Application.CQRS.Commands;

namespace Property.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PropertyController(IMediator mediator):Controller
{
   [HttpPost("CreateProperty")] 
   public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyCommand command)
   {
      var result = await mediator.Send(command);
      return result.IsSuccess ? Ok(result) : BadRequest(result);
   }
}