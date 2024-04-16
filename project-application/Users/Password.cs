// this class represents a password in its entirety and will take care of password logic
internal class Password
{
    //private string HashedPassword {get; set;}

    private string pass;
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
        throw new NotImplementedException();
    }

    // checks if two passwords match (to authenticate the user)
    public bool DoPasswordsMatch(string passwordToVerify)
    {
        throw new NotImplementedException();
    }
}