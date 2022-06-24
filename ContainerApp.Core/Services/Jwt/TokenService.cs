using ContainerApp.Contracts.Config;
using ContainerApp.Contracts.Data.Entities;
using ContainerApp.Contracts.DTO;
using ContainerApp.Contracts.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ContainerApp.Core.Services.Jwt
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenConfig _config;

        public TokenService(JwtTokenConfig tokenConfig)
        {
            _config = tokenConfig;
        }

        public AuthTokenDTO Generate(User user)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim (ClaimTypes.Role, user.Role.ToString())
            };

            JwtSecurityToken token = new TokenBuilder()
            .AddAudience(_config.Audience)
            .AddIssuer(_config.Issuer)
            .AddExpiry(_config.ExpiryInMinutes)
            .AddKey(_config.key)
            .AddClaims(claims)
            .Build();

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthTokenDTO
            {
                AccessToken = accessToken,
                ExpiresIn = _config.ExpiryInMinutes
            };
        }
    }
}