public interface ILLMClients
{
    public Task<string> sendPromptAsync(string prompt);
}