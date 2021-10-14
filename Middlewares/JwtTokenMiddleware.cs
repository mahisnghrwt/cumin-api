using cumin_api.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cumin_api.Middlewares {
    public class JwtTokenMiddleware {
        private readonly RequestDelegate next;

        public JwtTokenMiddleware(RequestDelegate next) {
            this.next = next;
        }

        private async Task ReturnErrorResponse(HttpContext context, Object message) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(JsonSerializer.Serialize(message));
        }

        public async Task Invoke(HttpContext context, TokenHelper tokenHelper) {
            string token = context.Request.Headers["Authorization"].ToString().Split(" ")?.Last();
            bool isHubPath = context.Request.Path.StartsWithSegments("/notification");
            
            if (!String.IsNullOrEmpty(token)) {
                var claims = tokenHelper.ExtractClaimsFromToken(token);
                if (claims != null) {
                    var userId = claims.First(x => x.Type == "userId").Value;
                    context.Items["userId"] = userId;
                    if (userId == null && isHubPath) {
                        await ReturnErrorResponse(context, new { message = "Invalid token." });
                        return;
                    }
                }
                else if (isHubPath) {
                    await ReturnErrorResponse(context, new { message = "Invalid token." });
                    return;
                }
            }
            else if (isHubPath){
                await ReturnErrorResponse(context, new { message = "Missing access_token in request header." });
                return;
            }
            await next(context);
        }



        
    }
}

// has token
// has claims

