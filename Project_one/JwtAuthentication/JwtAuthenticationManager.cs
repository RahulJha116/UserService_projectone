using Microsoft.IdentityModel.Tokens;
using Project_one.DbContexts;
using Project_one.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project_one.JwtAuthentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key;
        UserContext db = new UserContext();

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public string Authenticate(string userEmail, string password)
        {
            if(!db.Users.Any(u => u.EmailId == userEmail && u.Passkey == password))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userEmail)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenkey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


        }
    }
}
