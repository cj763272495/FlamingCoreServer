using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class TokenGenerator {
    private readonly string _issuer = "your_issuer"; // 例如 "MyGameAuth";
    private readonly string _audience = "your_audience"; // 例如 "MyGameUsers";

    public string GenerateJwtToken(string userName) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userName)
            }),
            Expires = DateTime.UtcNow.AddHours(1), // 设置令牌有效期为1小时
            Issuer = _issuer,
            Audience = _audience
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}