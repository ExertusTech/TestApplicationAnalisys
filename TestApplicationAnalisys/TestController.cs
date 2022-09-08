using Microsoft.AspNetCore.Mvc;

namespace TestApplicationAnalisys;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok("prueba");
    }

}