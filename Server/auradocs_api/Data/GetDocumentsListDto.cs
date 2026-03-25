public class GetDocumentsListDto
{
    public string? searchValue { get; set; } = String.Empty;
    public int? DocumentType { get; set; } = null;
    public int? DocumentStatus { get; set; } = null;
    public string? DocumentSharedBy { get; set; }
    public DateTime? CreatedOn { get; set; } = null;
}