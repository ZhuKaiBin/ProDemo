using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(ILogger<LoginController> logger, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(string name, string pwd)
        {
            var loginRet = await _signInManager.PasswordSignInAsync(name, pwd, true, true);

            return loginRet;
        }
    }
}