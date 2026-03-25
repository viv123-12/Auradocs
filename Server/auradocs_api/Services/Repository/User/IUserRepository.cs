using auradocs_api.Models;

public interface IUSerRepository
{
    public Task<string> GetUserNameUsingIdAsync(int userId);
    public Task<User> GetUserWihIdAsync(string userGuid);
}