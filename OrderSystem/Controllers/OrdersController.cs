using Microsoft.AspNetCore.Mvc;

namespace OrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Orders API is working!");
        }
    }
}