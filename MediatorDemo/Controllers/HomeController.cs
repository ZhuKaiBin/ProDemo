using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using MediatorDemo.Services;

namespace MediatorDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {

        private IServices _servicesA;
        private IServices _servicesB;
        //public HomeController([Named("ServicesA")] IServices services)
        //{
        //    _services = services;
        //}

        public HomeController(IServiceProvider serviceProvider)
        {
            _servicesA = serviceProvider.GetService<Services_A>();
            _servicesB = serviceProvider.GetService<Services_B>();
        }


        [HttpGet]
        public IActionResult Index()
        {
            _servicesA.Ret();
            _servicesB.Ret();
            return View();
        }
    }
}
