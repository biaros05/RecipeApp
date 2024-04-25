// this class represents a password in its entirety and will take care of password logic
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
// using RNGCryptoServiceProvider;
namespace users;
public static class Password
{
    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException("password cannot be empty");
        }
        if (password.Length < 5 || password.Length > 50)
        {
            throw new Exception("password doesnt meet requirements");
        }
    }

    // implements algo for hashing and returns the hashed password for that user
    public static string HashPassword(byte[] salt, string password)
    {
        ValidatePassword(password);
        int numIterations = 1000;

        HashAlgorithmName algoName = HashAlgorithmName.SHA512;

        Rfc2898DeriveBytes key = new(password, salt, numIterations, algoName);
        byte[] hash = key.GetBytes(32);
        string result = Convert.ToBase64String(hash);
        return result;
    }

    // checks if two passwords match (to authenticate the user)
    public static byte[] GenerateSalt()
    {
        byte[] Salt = new byte[8];
        // using RNGCryptoServiceProvider rngCsp = new();
        var random = new RNGCryptoServiceProvider();
        random.GetBytes(Salt);

        return Salt;
    }

    public static bool DoPasswordsMatch(string password,byte[] salt, string hash)//salt and hash
    {
        //do hash and salt 
        string enteredHash = HashPassword(salt, password);
        if (hash == enteredHash)
        {
            return true;
        }
        return false;
    }
}