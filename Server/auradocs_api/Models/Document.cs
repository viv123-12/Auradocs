public class Document
{
    public int uId { get; set; }
    public required string strGuid { get; set; }
    public required string strTitle { get; set; }
    public required int uDocumentType { get; set; }
    public required string strContent { get; set; }
    public string? strFileUrl { get; set; }
    public string? strFileType { get; set; }
    public int uStatusId { get; set; }
    public int uCurrentVersionId { get; set; }
    public int uCreatedBy { get; set; }
    public int uOwnerUserId { get; set; }
    public bool boolIsDeleted { get; set; } = false;
    public DateTime dtUpdatedOn { get; set; } = DateTime.UtcNow;
    public DateTime dtCreatedOn { get; set; }
}