using System.Security.Cryptography;

public class RandomStringGenerationService()
{

    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lower = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string Special = "!@#$%^&*()-_=+[]{}<>?";

    public static string GenerateRandomString(int length = 12)
    {
        if (length < 8)
            throw new ArgumentException("Password length must be at least 8");

        string allChars = Upper + Lower + Digits + Special;
        char[] password = new char[length];

        password[0] = Upper[GetRandomIndex(Upper.Length)];
        password[1] = Lower[GetRandomIndex(Lower.Length)];
        password[2] = Digits[GetRandomIndex(Digits.Length)];
        password[3] = Special[GetRandomIndex(Special.Length)];

        for (int i=4; i<length; i++)
        {
            password[i] = allChars[GetRandomIndex(length)];
        }

        return new string(password);
    }

     private static int GetRandomIndex(int max)
    {
        return RandomNumberGenerator.GetInt32(max);
    }
}