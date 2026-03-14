using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class UserRepository:IUSerRepository
{
    AuradocsContext _auradocsContext;
    public UserRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }

    public async Task<string?> GetUserNameUsingIdAsync(int userId)
    {
        string? userName = await _auradocsContext.Users.Where(u => u.uUid == userId && u.boolIsUserActivated).Select(u => u.strFullName).FirstOrDefaultAsync();
        return userName;
    }
}