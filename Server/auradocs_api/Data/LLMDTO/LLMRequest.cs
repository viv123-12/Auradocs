public class LLMrequest
{
    public required string Model { get; set; }
    public required List<LLMMessage> Messages { get; set; }
}