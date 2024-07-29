using Microsoft.AspNetCore.Mvc;
using WebAppAOPDemo.Core;

namespace WebAppAOPDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutoFacAopDemoController : ControllerBase
    {
        private readonly ILogger<AutoFacAopDemoController> _logger;
        private readonly IUserService _userService;

        public AutoFacAopDemoController(
            ILogger<AutoFacAopDemoController> logger,
            IUserService userService
        )
        {
            _logger = logger;
            _userService = userService;
        }

        //https://localhost:7232/AutoFacAopDemo/
        [HttpGet]
        public string Index()
        {
            //{Castle.Proxies.IUserServiceProxy}
            //
            _userService.GetUserInfo(1);
            _userService.GetUserName(1);
            return "okk";
        }
    }
}
