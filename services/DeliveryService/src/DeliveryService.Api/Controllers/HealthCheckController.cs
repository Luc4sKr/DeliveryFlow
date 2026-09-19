using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetHealthCheck()
        {
            return Ok(new { status = "Healthy" });
        }
    }
}
