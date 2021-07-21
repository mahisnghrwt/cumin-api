using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cumin_api.Models;
using cumin_api.Services;
using Microsoft.AspNetCore.Authorization;
using cumin_api.Attributes;

namespace cumin_api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private IUserService userService;

        public AuthController(IUserService userService) {
            this.userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserAuthenticationRequest authReq) {
            bool success = userService.RegisterUser(authReq.Username, authReq.Password);
            if (success == false)
                return NotFound( new { message = "Could not register user!" });

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAuthenticationRequest authReq) {
            string token = userService.Authenticate(authReq.Username, authReq.Password, HttpContext.Connection.RemoteIpAddress.ToString());
            if (token == null)
                return new NotFoundResult();

            return Ok( new { username = authReq.Username, token = token } );
        }

        [CustomAuthorization]
        [HttpGet("validate")]
        public IActionResult Validate() {
            try {
                bool casted = int.TryParse(HttpContext.Items["userId"].ToString(), out int userId);
                if (!casted)
                    return NotFound();

                var user = userService.GetUser(userId);
                return Ok(user);
            } catch { }
            return NotFound();
        }
    }
}
