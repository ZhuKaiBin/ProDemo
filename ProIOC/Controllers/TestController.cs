using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        //private ITestServices _service;
        //public TestController(ITestServices services)
        //{
        //    _service = services;
        //}

        //public Tuple<int, int> Get([FromServices]ITestServices tservices)
        //{  
        //    int before = tservices.count;
        //    tservices.Add();
        //    int after = tservices.count;
        //    return new Tuple<int, int>(before, after);
        //}

        public TestServiceTemp _services;
        public TestController(TestServiceTemp services)
        {
            _services = services;
        }

        public Tuple<int, int> Get()
        {
            int before = _services.count;
            _services.Add();
            int after = _services.count;
            return new Tuple<int, int>(before, after);
        }

    }
}
