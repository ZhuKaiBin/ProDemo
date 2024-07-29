using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProIOC
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutofacController : ControllerBase
    {
        public IConstructor _constructor;

        public AutofacController(IConstructor constructor)
        {
            _constructor = constructor;
        }

        public Tuple<int, int> Get()
        {
            int before = _constructor.service.count;
            _constructor.service.Add();
            int after = _constructor.service.count;

            return new Tuple<int, int>(before, after);
        }
    }
}
