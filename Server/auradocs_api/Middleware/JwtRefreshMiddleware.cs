using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using auradocs_api.Services;

namespace auradocs_api.Middleware;
public class JwtRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly JWTService _jwtService;

    public JwtRefreshMiddleware(RequestDelegate next, IConfiguration config, JWTService jWTService)
    {
        _next = next;
        _config = config;
        _jwtService = jWTService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/User") || context.Request.Path.StartsWithSegments("/MasterData"))
        {
            await _next(context);
            return;
        }
        var token = context.Request.Cookies["auradocs_access_token"];
        if(string.IsNullOrWhiteSpace(token))
        {
            //user will be logged out if no cookie present
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;            
            context.Response.Cookies.Delete("auradocs_access_token");
            return;
        }
        List<Claim> claims= _jwtService.GetPrincipalClaims(token);
        if(claims.Count == 0)
        {
            //user will be logged out if no token doesn't contain the claims
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;            
            context.Response.Cookies.Delete("auradocs_access_token");
            return;
        }
        var refreshedToken = _jwtService.GenerateAccessToken(claims);
        context.Response.Cookies.Append(
            "auradocs_access_token",
            refreshedToken
            ,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("CookieExpirationTime"))
            }
        );
        await _next.Invoke(context);
    }
}