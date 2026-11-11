using System.Text;
using API.Entities;
using API.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
           var tokenKey = config["TokenKey"]?? throw new Exception("Cannot get token key");
           if(tokenKey.Length<64) 
           throw new Exception("Your token key needs to be >=64 chars");

           //var key = new SymmetricSecurityKey();
           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

           var claims = new List<Claim>
           {
                new(ClaimTypes.Email, appUser.Email),
                new(ClaimTypes.NameIdentifier, appUser.Id)
           };
           
           var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

           var tokenDescriptor = new SecurityTokenDescriptor
           {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
           };

           var tokenHandler = new JwtSecurityTokenHandler();
           var token = tokenHandler.CreateToken(tokenDescriptor);
           
           return tokenHandler.WriteToken(token);
        }
    }
}