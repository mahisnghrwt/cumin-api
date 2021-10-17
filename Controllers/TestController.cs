using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase {
        public TestController() {}

        [HttpGet]
        public IActionResult Hello() {
            return Ok(new { message = "Hello!" });
        }

    }
}
