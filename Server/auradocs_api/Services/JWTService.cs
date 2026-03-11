using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace auradocs_api.Services;

public class JWTService{

    private IConfiguration _config;
    private IHttpContextAccessor _httpContextAccessor;
    public JWTService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }
    public string GenerateAccessToken(string email, string userId, UserRole role)
    {
        var JwtConfig = _config.GetSection("JwtSettings");
        var claims = new[]
        {
            new Claim("UserId",userId.ToString()),
            new Claim("Email",email),
            new Claim("UserType",role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var expiryTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(JwtConfig["ExpiryMinutes"]));
        var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig["key"])),SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            issuer: JwtConfig["Issuer"],
            audience: JwtConfig["Audience"],
            claims: claims,
            expires: expiryTime,
            signingCredentials: securityKey
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var JwtConfig = _config.GetSection("JwtSettings");
        var expiryTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(JwtConfig["ExpiryMinutes"]));
        var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig["key"])),SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            issuer: JwtConfig["Issuer"],
            audience: JwtConfig["Audience"],
            claims: claims,
            expires: expiryTime,
            signingCredentials: securityKey
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public List<Claim> GetPrincipalClaims(string token)
    {
        try
        {
            List<Claim> claims = new List<Claim>();
            var jwt = _config.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwt["Key"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            claims.AddRange(principal.Claims);
            return claims;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public string GetToken()
    {
        string token  = _httpContextAccessor.HttpContext?.Request.Cookies[AppConstants.Auradocs_access_token_key];
        return token;
    }
}