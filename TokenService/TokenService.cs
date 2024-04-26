using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project_v1.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_v1.TokenService {
    public class TokenService : ITokenService {

        private readonly UserManager<SystemUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config, UserManager<SystemUser> userManager) {
            _userManager = userManager;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:TokenKey"]));
        }

        public string CreateToken(SystemUser user) {
            var claims = new List<Claim> {
                new Claim("Username", user.UserName),
                new Claim("UserId", user.Id)
            };

            var roles = _userManager.GetRolesAsync(user).Result;

            foreach (var role in roles) {
                claims.Add(new Claim("Role", role));
            }
            
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
