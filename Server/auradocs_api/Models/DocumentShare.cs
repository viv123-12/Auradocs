public class DocumentSharedWithUser
{
    public int uId { get; set; }
    public required int uSharedDocumentId { get; set; }
    public required int uSharedWith { get; set; }
    public required int uSharedBy { get; set; }
    public required int uAccessgiven { get; set; }
    public required DateTime dtAccessGivenOn { get; set; }
}