using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Property.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PropertyTypeController(IMediator mediator):Controller
{
    
}