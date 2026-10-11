using System.Security.Claims;
using System.Text;
using LiveAuction.Application.Abstractions;
using LiveAuction.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LiveAuction.Infrastructure.Auth;

public class JwtTokenGenerator(IOptions<JwtOptions> options, TimeProvider clock)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    public string Generate(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.DisplayName)
            }),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Expires = clock.GetUtcNow().UtcDateTime.AddMinutes(_options.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}
