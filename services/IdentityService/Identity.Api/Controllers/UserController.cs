using Identity.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator): Controller
{
   [HttpPut("update-host-profile")]
   public async Task<IActionResult> UpdateHostProfile([FromBody] UpdateUserProfileCommand command)
   {
      var result = await mediator.Send(command);
      return result.Success? Ok(result) : BadRequest(result);
   }
}