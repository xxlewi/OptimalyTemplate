using Microsoft.AspNetCore.Mvc;
using OT.ServiceLayer.Exceptions;

namespace OT.PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("test-not-found")]
    public IActionResult TestNotFoundException()
    {
        _logger.LogInformation("Testování NotFoundException");
        throw new NotFoundException("TestEntity", 123);
    }

    [HttpGet("test-validation")]
    public IActionResult TestValidationException()
    {
        _logger.LogInformation("Testování ValidationException");
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email je povinný", "Email není ve správném formátu" } },
            { "Name", new[] { "Jméno je povinné" } }
        };
        throw new ValidationException(errors);
    }

    [HttpGet("test-business")]
    public IActionResult TestBusinessException()
    {
        _logger.LogInformation("Testování BusinessException");
        throw new BusinessException("Nelze vykonat tuto operaci z business důvodů", "BUSINESS_RULE_VIOLATION");
    }

    [HttpGet("test-general")]
    public IActionResult TestGeneralException()
    {
        _logger.LogInformation("Testování obecné výjimky");
        throw new InvalidOperationException("Obecná výjimka pro testování");
    }

    [HttpGet("test-success")]
    public IActionResult TestSuccess()
    {
        _logger.LogInformation("Test úspěšného volání");
        return Ok(new { Message = "Vše funguje správně!", Timestamp = DateTime.UtcNow });
    }
}