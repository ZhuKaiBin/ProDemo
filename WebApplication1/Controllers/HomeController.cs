using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }

        //[Authorize(Roles = "SVIP")]
        //public string SVIP()
        //{
        //    return "SVIP";
        //}
        //[Authorize(Roles = "SVIP,VIP")]
        //public string VIP()
        //{
        //    return "VIP";
        //}
        //[Authorize(Roles = "SVIP,VIP,NoVIP")]
        //public string NoVIP()
        //{
        //    return "NoVIP";
        //}

        [Authorize(Policy = "SVIP")]
        public string SVIP()
        {
            return "SVIP";
        }

        [Authorize(Policy = "VIP")]
        public string VIP()
        {
            return "VIP";
        }

        [Authorize(Policy = "NoVIP")]
        public string NoVIP()
        {
            return "NoVIP";
        }
    }
}
