// this class represents a password in its entirety and will take care of password logic
using RNGCryptoServiceProvider;
namespace users;
public class Password
{
    //private string HashedPassword {get; set;}

    private string pass;
    public byte[] salt = new byte[8];
    using RNGCryptoServiceProvider rngCsp = new();
    RngCsp.GetBytes(salt);

    
    

    public string Pass {
        get{
            return this.pass;
        }
        set{
            if (value.Length<5 || value.Length>50)
            {
                throw new Exception("the password length must be between 5-50");
            }
            //can i use = or do i need .Equals()
            this.pass=value;
        }
        }
    public Password(string password)
    {
        this.Pass=password;
    }

    // implements algo for hashing and returns the hashed password for that user
    private string HashPassword()
    {
        private int numIterations = 1000;

        HashAlgorithmName algoName = HashAlgorithmName.SHA512;

        Rfc2898DeriveBytes key =new(pass, salt, numIterations, algoName);
        byte[] hash = key.GetBytes(32);
    }

    // checks if two passwords match (to authenticate the user)
    public bool DoPasswordsMatch(string passwordToVerify)
    {
        throw new NotImplementedException();
    }

    // public static implicit operator Password(string v)
    // {
    //     throw new NotImplementedException();
    // }
}