using Microsoft.AspNetCore.Mvc;


namespace WaiterApp.Controllers;

[ApiController]
[Route("heathcheck")]
public class HeathcheckController: ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> HeathCheck()
    {
        return Ok("ok");
    }
}
