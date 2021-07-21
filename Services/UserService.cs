using cumin_api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cumin_api.Services {
    public interface IUserService {
        string Authenticate(string username, string password, string remoteIp);
        bool RegisterUser(string username, string password);
        User GetUser(int userId);
    }

    public class UserService : IUserService {
        private readonly CuminApiContext dbContext;
        private readonly SecurityConfiguration securityConfig;

        public UserService(CuminApiContext dbContext, IOptions<SecurityConfiguration> securityConfig) {
            this.dbContext = dbContext;
            this.securityConfig = securityConfig.Value;
        }
        
        public bool RegisterUser(string username, string password) {
            var user = dbContext.Users.FirstOrDefault(user => user.Username == username);
            // User already exists in the database
            if (user != null)
                return false;
            dbContext.Users.Add(new Models.User { Username = username, Password = password });
            if (dbContext.SaveChanges() <= 0)
                return false;

            return true;
        }
        public string Authenticate(string username, string password, string remoteIp) {
            var user = dbContext.Users.FirstOrDefault(user => user.Username == username && user.Password == password);
            if (user == null)
                return null;

            return GenerateToken(user.Id, remoteIp);
        }

        private string GenerateToken(int userId, string remoteIp) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim [] { new Claim("userId", userId.ToString()), new Claim("remoteIp", remoteIp) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User GetUser(int userId) {
            return dbContext.Users.FirstOrDefault(user => user.Id == userId);
        }
    }
}
