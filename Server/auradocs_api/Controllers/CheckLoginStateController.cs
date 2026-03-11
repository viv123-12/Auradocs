using Microsoft.AspNetCore.Mvc;
namespace auradocs_api.Controllers;

[ApiController]
[Route("[Controller]")]
public class CheckLoginStateController : ControllerBase
{
    public CheckLoginStateController()
    {}

    
    [HttpGet("check-login-state")]
    public async Task<IActionResult> CheckLoginAsync()
    {
        return Ok();
    } 
}