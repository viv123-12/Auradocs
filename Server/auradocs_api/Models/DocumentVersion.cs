public class DocumentVersion
{
    public int uId { get; set; }
    public required string strGuid { get; set; }
    public int uVersion { get; set; }
    public int uDocumentId { get; set; }
    public required string strContent { get; set; }
    public int uUpdatedBy { get; set; }
    public DateTime dtUpdatedOn { get; set; }
}