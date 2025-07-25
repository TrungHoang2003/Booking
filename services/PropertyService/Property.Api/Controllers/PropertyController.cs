using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Property.Application.UseCases.Commands;

namespace Property.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PropertyController(IMediator mediator): Controller
{
}