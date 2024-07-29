using Microsoft.AspNetCore.Mvc;
using WebAppAOPDemo.FilterDemo;

namespace WebAppAOPDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttributeDemoController : Controller
    {
        //[CustomerLogAopAsync]
        [CustomerCacheAop]
        [CustomerLogAop]
        [HttpGet]
        public string Index()
        {
            return "CustomerLogAop";
        }
    }
}
