using Microsoft.AspNetCore.Mvc;
using System;
using cumin_api.Models;
using cumin_api.Attributes;
using Microsoft.EntityFrameworkCore;

namespace cumin_api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly Services.v2.UserService userService;
        private readonly Others.TokenHelper tokenHelper;

        public AuthController(Services.v2.UserService userService, Others.TokenHelper tokenHelper) {
            this.userService = userService;
            this.tokenHelper = tokenHelper;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserAuthenticationDto userAuthDto) {
            User user = new User { Username = userAuthDto.Username, Password = userAuthDto.Password };
            User user_;
            try {
                user_ = userService.Add(user);
            } catch (Exception e) {
                throw e;
            }

            return Ok(user_);
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] UserAuthenticationDto authReq) {
            var user = userService.Find(x => x.Username == authReq.Username && x.Password == authReq.Password);
            if (user == null)
                return new UnauthorizedResult();

            string token = tokenHelper.GenerateToken(user.Id, HttpContext.Connection.RemoteIpAddress.ToString());
            var user_ = userService.GetWithActiveProject(user.Id);
            // get the user details

            return Ok( new { user = user_, token = token } );
        }

        [CustomAuthorization]
        [HttpGet]
        public IActionResult ValidateToken() {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            try {
                return Ok(userService.GetWithActiveProject(userId));
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [HttpGet("hello")]
        public IActionResult Hello() {
            return Ok(new { message = "Hello"});
        }
    }
}
