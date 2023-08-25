using BlazorBattles.Server.Config;
using BlazorBattles.Server.Services.Contracts;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorBattles.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly APISettings _apiSettings;
        private readonly UserManager<User> _userManager;

        public TokenService(IOptions<APISettings> options, UserManager<User> userManager)
        {
            _apiSettings = options.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
            _userManager = userManager;
        }
        public string CreateToken(User user)
        {
            var signingCreds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var claims = GetClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signingCreds,
                Issuer = _apiSettings.ValidIssuer,
                Audience = _apiSettings.ValidAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>();

            if (user != null)
            {

                claims.Add(new Claim(ClaimTypes.Name, user.Email));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim("Id", user.Id));

                var userFromDb = _userManager.FindByEmailAsync(user.Email).GetAwaiter().GetResult();
                if (userFromDb != null)
                {
                    var roles = _userManager.GetRolesAsync(userFromDb).GetAwaiter().GetResult();

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

            }

            return claims;
        }
    }
}
