public class CreateDocumentDto
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public string? FolderId { get; set; }
    public required int Status { get; set; }
}