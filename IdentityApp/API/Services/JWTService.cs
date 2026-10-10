using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;

        public JWTService(IConfiguration config)
        {
            _config = config;
            var jwtKeyValue = _config["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKeyValue))
            {
                throw new ArgumentException("JWT:Key configuration value is missing or empty.", nameof(config));
            }
            _jwtKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKeyValue));
        }

        public string CreateJWT(User user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty)
                //new Claim("My CustomClaim", "CustomValue")

            };

            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha512Signature);

            var expiresInDaysValue = _config["JWT:ExpiresInDays"];
            //var expiresInDays = !string.IsNullOrEmpty(expiresInDaysValue) ? int.Parse(expiresInDaysValue) : 1;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(expiresInDaysValue)),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"],
                //Audience = _config["JWT:Audience"]
            };   
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
