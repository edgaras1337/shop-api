using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Helpers
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtSettings;

        public JwtService(IOptions<JwtConfiguration> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(
                    JwtRegisteredClaimNames.Nbf,
                    new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(
                    JwtRegisteredClaimNames.Exp,
                    new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            foreach (var userRole in user.UserRoles)
            {
                if(userRole.Role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                }
            }

            var symmetricSecurity = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(symmetricSecurity, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(claims);

            var token = new JwtSecurityToken(header, payload);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
