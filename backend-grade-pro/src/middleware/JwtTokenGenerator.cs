using backend_grade_pro.src.models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_grade_pro.src.middleware
{
    public class JwtTokenGenerator
    {
        public static string GenerateToken(User user, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                   new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                   new Claim(ClaimTypes.Name, user.Email),
                   new Claim(ClaimTypes.Role, user.Role.Name)
               }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
