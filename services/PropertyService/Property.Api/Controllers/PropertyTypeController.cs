using MediatR;
using Microsoft.AspNetCore.Mvc;
using Property.Application.CQRS.Queries;

namespace Property.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PropertyTypeController(IMediator mediator):Controller
{
    [HttpGet("GetPropertyTypes")]
    public async Task<IActionResult> GetPropertyTypes(GetPropertyTypesQuery query)
    {
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    
}