namespace auradocs_api.Models;
using System;

public class User
{
    public int uUid {get; set;}
    public required string strGuid {get; set;}
    public required string strUserId {get; set;}
    public string? strPassword {get;set;}
    public string? strPasswordSalt {get; set;}
    public required UserRole strUserRole {get; set;}
    public required string strphoneNumber {get; set;}
    public required string strAccountType {get; set;}
    public required int uDomainType {get; set;}
    public required int uPracticeArea {get; set;}
    public string? strFullName {get; set;}
    public string? strProfilePictureURL {get; set;}
    public string? strAddress {get; set;}
    public string? strDescription {get; set;}
    public string? strOrganizationName {get; set;}
    public string? strJobTitle {get; set;}
    public bool boolIsUserActivated {get; set;}
    public DateTime? dtLastLogin {get; set;} =  null;
    public DateTime dtAdded {get; set;} = DateTime.UtcNow;

}