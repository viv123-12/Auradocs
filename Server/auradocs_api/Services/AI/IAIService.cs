namespace auradocs_api.Services;
public interface IAIService
{
    public string geneatePromptString(LLMPromptTemplate lLMPromptTemplate);
    public Task<string> RewriteAsync(string selectedText, string documentContext);
    public Task<string> SummerizeAsync(string selectedText, string documentContext);
    public Task<string> TranslateAsync(string selectedText, string language);
}