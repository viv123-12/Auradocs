public class DocumentResponse
{
    public string documentId { get; set; }
    public string documentTitle { get; set; }
    public string documentContent { get; set; }
    public string documentState { get; set; }
    public string? ownedBy { get; set; }
    public string? createdBy { get; set; }
    public int currentDocumentVersion { get; set; }
}