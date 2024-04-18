// this class represents a password in its entirety and will take care of password logic
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
// using RNGCryptoServiceProvider;
namespace users;
public class Password
{
    //private string HashedPassword {get; set;}

    public byte[] Salt = new byte[8];
    

    
    
    public string pass;

    private string Hash;

    //pass shoudnt be saved
    // private string Pass {
    //     get{
    //         return this.pass;
    //     }
    //     set{
    //         if (value.Length<5 || value.Length>50)
    //         {
    //             throw new Exception("the password length must be between 5-50");
    //         }
    //         //can i use = or do i need .Equals()
    //         this.pass=value;
    //     }
    //     }
    public Password(string password)
    {
        if (password.Length<5 || password.Length>50)
        {
            byte[] salt=GenerateSalt();
            this.Hash=HashPassword(salt,password);
            this.Salt=salt;
        }
        else{
            throw new Exception("password doesnt meet requirements");
        }
        // this.Pass=password;
    }

    // implements algo for hashing and returns the hashed password for that user
    private string HashPassword(byte[] salt,string password)
    {
        int numIterations = 1000;

        HashAlgorithmName algoName = HashAlgorithmName.SHA512;

        Rfc2898DeriveBytes key =new(password, salt, numIterations, algoName);
        byte[] hash = key.GetBytes(32);
        string result=Convert.ToBase64String(hash);
        return result;
    }

    // checks if two passwords match (to authenticate the user)
    public byte[] GenerateSalt()
    {
        // using RNGCryptoServiceProvider rngCsp = new();
        var random = new RNGCryptoServiceProvider();
        random.GetBytes(Salt);

        return Salt;
    }

    public bool DoPasswordsMatch(string passwordToVerify)
    {
        throw new NotImplementedException();
    }

    // public static implicit operator Password(string v)
    // {
    //     throw new NotImplementedException();
    // }
}