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
        RequestDelegate next;
        SecurityConfiguration securityConfig;

        public JwtTokenMiddleware(RequestDelegate next, IOptions<SecurityConfiguration> securityConfig) {
            this.securityConfig = securityConfig.Value;
            this.next = next;
        }

        public async Task Invoke(HttpContext context) {
            var token = context.Request.Headers["Authorization"].ToString().Split(" ")?.Last();
            if (token != null)
                AttachTokenToContext(context, token);
            await next(context);
        }

        public void AttachTokenToContext(HttpContext context, string token) {
            try {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(securityConfig.Secret);
                var validationParameters = new TokenValidationParameters {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "userId").Value;
                context.Items["userId"] = userId;
            } catch {

            }
        }
    }
}
