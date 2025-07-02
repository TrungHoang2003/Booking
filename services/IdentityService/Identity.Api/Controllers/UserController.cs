using Identity.Application.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator): Controller
{
   [HttpPut("UpdateHostProfile")]
   public async Task<IActionResult> UpdateHostProfile([FromBody] UpdateUserProfileCommand command)
   {
      var result = await mediator.Send(command);
      return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
   }
}