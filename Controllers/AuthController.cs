using Microsoft.AspNetCore.Mvc;
using System;
using cumin_api.Models;
using cumin_api.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<IActionResult> RegisterUser([FromBody] UserAuthenticationDto userAuthDto) {
            User user = new User { Username = userAuthDto.Username, Password = userAuthDto.Password };
            try {
                User user_ = await userService.AddAsync(user);
                return Ok(user_);
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] UserAuthenticationDto authReq) {
            var user = userService.Find(x => x.Username == authReq.Username && x.Password == authReq.Password);
            if (user == null)
                return new UnauthorizedObjectResult(new { message = "User not found or incorrect password."});

            string token = tokenHelper.GenerateToken(user.Id);
            var user_ = userService.GetWithActiveProject(user.Id);

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
    }
}
