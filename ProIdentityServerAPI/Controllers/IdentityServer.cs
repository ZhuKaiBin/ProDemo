using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIdentityServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityServer : ControllerBase
    {
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult
                   (from claim in User.Claims
                    select new { claim.Type, claim.Value }
                    );
        }
    }
}
