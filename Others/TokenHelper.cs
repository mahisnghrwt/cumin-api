using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cumin_api.Others {
    public class TokenHelper {
        private readonly SecurityConfiguration securityConfig;
        public TokenHelper(IOptions<SecurityConfiguration> securityConfig) {
            this.securityConfig = securityConfig.Value;
        }
        public IEnumerable<System.Security.Claims.Claim> ExtractClaimsFromToken(string token) {
            if (token == null)
                return null;

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
                return jwtToken.Claims;
            } catch (Exception e){
                
            }

            return null;
        }

        public string GenerateToken(int userId, string remoteIp) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityConfig.Secret);
            //var createdOn = DateTime.Now;
            //var expiryOn = createdOn.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] { new Claim("userId", userId.ToString()), new Claim("remoteIp", remoteIp) }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
