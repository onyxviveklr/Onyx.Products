using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Onyx.ProductsApi.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [Route("api/[controller]")]
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok("Onyx Products API is running");
        }
    }
}
