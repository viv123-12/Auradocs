public class UploadDocumentDto
{
    public required IFormFile File { get; set; }
    public required string Title { get; set; }
    public required string Filetype { get; set; }
    public string DocumentType { get; set; }
    public string? FolderId { get; set; }
}