using cumin_api.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumin_api.Middlewares {
    public class JwtTokenMiddleware {
        private readonly RequestDelegate next;

        public JwtTokenMiddleware(RequestDelegate next) {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, TokenHelper tokenHelper) {
            var token = context.Request.Headers["Authorization"].ToString().Split(" ")?.Last();
            if (token != null) {
                var claims = tokenHelper.ExtractClaimsFromToken(token);
                if (claims != null) {
                    var userId = claims.First(x => x.Type == "userId").Value;
                    context.Items["userId"] = userId;
                }
            }
            await next(context);
        }
    }
}
