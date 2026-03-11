using System.Security.Cryptography;

public static class HashingService
{
    public static (string Hash, string Salt) HashPassword(string password)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256
        );

        byte[] hashBytes = pbkdf2.GetBytes(32);

        return (
            Convert.ToBase64String(hashBytes),
            Convert.ToBase64String(saltBytes)
        );
    }

    public static bool VerifyPassword(
        string enteredPassword,
        string storedHash,
        string storedSalt
    )
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            enteredPassword,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256
        );

        byte[] enteredHash = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(
            enteredHash,
            Convert.FromBase64String(storedHash)
        );
    }
}
