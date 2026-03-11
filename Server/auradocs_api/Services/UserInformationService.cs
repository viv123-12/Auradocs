using System.Security.Claims;
using auradocs_api.Contexts;
using auradocs_api.Models;
using auradocs_api.Services;
using Microsoft.EntityFrameworkCore;

public class UserInformationService
{
    private readonly JWTService _jWTService;
    private readonly AuradocsContext _auradocsContext;
    public UserInformationService(JWTService jWTService, AuradocsContext auradocsContext)
    {
        _jWTService = jWTService;
        _auradocsContext = auradocsContext; 
    }

    public async Task<User> GetUserInformation()
    {
        Claim userId = await GetClaim("UserId");
        if (userId != null)
        {
           return await _auradocsContext.Users.Where(u => u.strGuid == userId.Value).FirstOrDefaultAsync();
        }
        return null;
    }

    private async Task<Claim> GetClaim(string claimType)
    {
        string token =  _jWTService.GetToken();
        if(string.IsNullOrEmpty(token))
        {
            return null;
        }

        List<Claim> claims = _jWTService.GetPrincipalClaims(token);
        if (claims.Count == 0)
        {
            return null;
        }
        Claim userId = claims.FirstOrDefault(e =>  e.Type == claimType);
        return userId;
    }
}