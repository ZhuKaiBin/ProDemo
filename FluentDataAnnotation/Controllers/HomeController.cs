using FluentDataAnnotation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluentDataAnnotation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpPost]
        public IActionResult Get(Model model)
        {
            return Ok();
        }
    }
}
