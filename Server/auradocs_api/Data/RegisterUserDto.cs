public class RegisterUserDto
{
    public string? userId {get; set;}
    public required string phoneNumber {get; set;}
    public required string AccountType {get; set;}
    public required int DomainType {get; set;}
    public required int PracticeArea {get; set;}
}