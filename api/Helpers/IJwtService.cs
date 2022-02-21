using api.Models;
using System.IdentityModel.Tokens.Jwt;

namespace api.Helpers
{
    public interface IJwtService
    {
        Task<string> Generate(User user);
    }
}
