using auradocs_api.Services;
using System.Text;
public class AIService : IAIService
{
    private readonly ILLMClients _llmClient;
    public AIService(ILLMClients llmClient)
    {
        _llmClient = llmClient;
    }
    public async Task<string> RewriteAsync(string selectedText, string documentContext)
    {
        LLMPromptTemplate llmPromptTemplate = new LLMPromptTemplate
        {
          SelectedText = selectedText,
          Instruction = "Fix grammar and improve clarity.",
          DocumentContext = documentContext,
          Tone = "Professional",
          Rules = ["Preserve original meaning", "Do not add new information", "Return only the rewritten text", "Do not explain anything"],
          Language = "English"
        };
        string rewritePrompt = geneatePromptString(llmPromptTemplate);
        return await _llmClient.sendPromptAsync(rewritePrompt);
    }

    public async Task<string> SummerizeAsync(string selectedText, string documentContext)
    {
        LLMPromptTemplate llmPromptTemplate = new LLMPromptTemplate
        {
          SelectedText = selectedText,
          Instruction = " Provide a concise summary of the selected text.",
          DocumentContext = documentContext,
          Tone = "Professional",
          Rules = new List<string>
                {
                    "Preserve the original meaning.",
                    "Focus only on the key points.",
                    "Keep the summary concise and clear.",
                    "Do not introduce new information.",
                    "Return only the summary text.",
                    "Do not explain anything."
                },
          Language = "English"
        };
        string summerizePrompt = geneatePromptString(llmPromptTemplate);
        return await _llmClient.sendPromptAsync(summerizePrompt);
    }

    public async Task<string> TranslateAsync(string selectedText, string language)
    {
        LLMPromptTemplate llmPromptTemplate = new LLMPromptTemplate
        {
          SelectedText = selectedText,
          Instruction = $"Translate the text into {language}",
          Tone = "professional",
          Rules = 
                [ 
                    "Preserve the original meaning.",
                    "Do not add new information.",
                    "Keep the tone consistent with the original text.",
                    "Return only the translated text.",
                    "Do not explain anything."
                ],
          Language = language
        };
        string translatePrompt = geneatePromptString(llmPromptTemplate);
        return await _llmClient.sendPromptAsync(translatePrompt);
    }

    public string geneatePromptString(LLMPromptTemplate lLMPromptTemplate)
    {
        StringBuilder promptBuilder = new StringBuilder();

        promptBuilder.AppendLine("Rules:");

        foreach (string rule in lLMPromptTemplate.Rules)
        {
            promptBuilder.AppendLine($"- {rule}");
        }

        string rulesSection = promptBuilder.ToString();
        string prompt = $@"You are an AI writing assistant.
                        Rewrite the given text based on the instruction.

                        Instruction: {lLMPromptTemplate.Instruction}

                        Context:
                        The text comes from a larger document. Use the context to maintain meaning but only rewrite the selected text.

                        Context Paragraph:
                        {lLMPromptTemplate.DocumentContext}

                        Selected Text:
                        {lLMPromptTemplate.SelectedText}

                        Rules:
                        {rulesSection}";
        return prompt;
    }
}