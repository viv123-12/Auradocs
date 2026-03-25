public class Folder
{
    public int uId {get; set;}
    public required string strGuid { get; set; }
    public required string strTitle { get; set; }
    public int? uParentFolderId { get; set; }
    public int uOwnerUserId { get; set; }
    public bool boolIsDeleted { get; set; } = false;
    public int uCreatedBy { get; set; }
    public DateTime dtCreatedOn { get; set; }
}