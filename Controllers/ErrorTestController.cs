using Microsoft.AspNetCore.Mvc;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/error")]
public class ErrorTestController : ControllerBase
{
    [HttpGet]
    public IActionResult ThrowError()
    {
        throw new InvalidOperationException("Simulated internal service structural failure.");
    }
}