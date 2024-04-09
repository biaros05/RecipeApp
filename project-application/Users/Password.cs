// this class represents a password in its entirety and will take care of password logic
internal class Password
{
    private string HashedPassword {get; set;}
    
    public Password(string password)
    {

    }

    // implements algo for hashing and returns the hashed password for that user
    private string HashPassword()
    {
        throw new NotImplementedException();
    }

    // checks if two passwords match (to authenticate the user)
    public bool DoPasswordsMatch(string passwordToVerify)
    {
        throw new NotImplementedException();
    }
}