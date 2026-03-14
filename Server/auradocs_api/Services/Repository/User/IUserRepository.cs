public interface IUSerRepository
{
    public Task<string> GetUserNameUsingIdAsync(int userId);
}