using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController(ILogger<TestController> logger) : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        logger.LogInformation("Health check endpoint called at {Timestamp}", DateTime.UtcNow);
        logger.LogWarning("This is a warning message for testing Seq");
        logger.LogError("This is an error message for testing Seq");
        
        return Ok(new { 
            Status = "Healthy", 
            Timestamp = DateTime.UtcNow,
            Message = "Check Seq for these log messages" 
        });
    }
    
    [HttpGet("test-seq")]
    public IActionResult TestSeq()
    {
        logger.LogInformation("Testing Seq logging with structured data: {@TestData}", new { 
            UserId = 123, 
            Action = "TestSeq", 
            RequestId = Guid.NewGuid() 
        });
        
        return Ok("Seq test logged - check Seq dashboard");
    }
}
