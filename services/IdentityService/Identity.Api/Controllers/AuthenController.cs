using Identity.Application.CQRS.Commands;
using Identity.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenController(IMediator mediator, ILogger<AuthenController> logger): Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        logger.LogInformation("Login attempt for user: {UserName}", command.UserName);
        var result = await mediator.Send(command);
        
        if (result.IsSuccess)
        {
            logger.LogInformation("Login successful for user: {UserName}", command.UserName);
            return Ok(result.Value);
        }
        
        logger.LogWarning("Login failed for user: {UserName}. Error: {Error}", command.UserName, result.Error);
        return BadRequest(result.Error);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        logger.LogInformation("Registration attempt for user: {Email}", command.Email);
        var result = await mediator.Send(command);
        
        if (result.IsSuccess)
        {
            logger.LogInformation("Registration successful for user: {Email}", command.Email);
            return Ok(result);
        }
        
        logger.LogWarning("Registration failed for user: {Email}. Error: {Error}", command.Email, result.Error);
        return BadRequest(result.Error);
    }
}