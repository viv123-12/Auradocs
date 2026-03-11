public class ResetPasswordToken
{
    public int uId { get; set; }
    public required string strGuid { get; set; }
    public required string strUserId { get; set; }
    public required string strToken { get; set; }
    public DateTime dtExpiresAt { get; set; }
    public required bool boolIsVerified { get; set; }
    public required bool boolIsUsed { get; set; } = false ;
    public DateTime dtCreatedAt { get; set; }
}