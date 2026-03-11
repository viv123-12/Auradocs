public class LLMPromptTemplate
{
    public required string SelectedText { get; set; }
    public required string Instruction { get; set; }
    public string? DocumentContext { get; set; }
    public required string Tone { get; set;}
    public required List<string> Rules { get; set; }
    public required string Language { get;  set; }
}