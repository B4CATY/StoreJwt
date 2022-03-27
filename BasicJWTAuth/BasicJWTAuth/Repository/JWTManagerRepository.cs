using BasicJWTAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BasicJWTAuth.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        Dictionary<string, string> UserRecords = new Dictionary<string, string>
        {
            {"user1", "user1" },
            {"user2", "user2" },
            {"user3", "user3" },
            {"user4", "user4" },

        };

        private readonly IConfiguration _configuration;

        public JWTManagerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Tokens Authenticate(Users user)
        {
            if(!UserRecords.Any(a => a.Key == user.Name && a.Value == user.Password))
            {
                return null;
            }
            //else generate token
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                    //new Claim(ClaimTypes.Role, "user"),

                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateJwtSecurityToken(tokendescriptor);
            return new Tokens { Token = tokenhandler.WriteToken(token) };
        }
    }
}
