using System.Collections.Generic;
namespace users;

using System.Net;
using recipes;
public class UserController
{
    // currently logged on user
    public static User ActiveUser {get; set;}
    public static List<User> AllUsers{get;} = new();

    
    // adds a new account (validates the input) --> should it take a user or params to create a new user individually?
    // public void CreateAccount(User newUser){}
    public void CreateAccount(string username, string password,string description)
    {
        foreach (User user in ListOfUsers)
        {
            if(user.Username.Equals(username))
            {
                throw new Exception("username already exists");
            }

            if(password.Length<5 || password.Length>50)
            {
                throw new Exception("username already exists");
            }
        }
        Password p=new Password(password);
        User user1 = new User(username,p,description);
        ListOfUsers.Add(user1);
    }
    // make sure the user exists in the database, and the hashed password matches 
    // the hashed password of the username 
    // (interacts with data layer to retrieve list of users to perform authentication)
    // update ActiveUser
    public bool AuthenticateUser(string username, string password)
    {
        throw new NotImplementedException();
    }

    // retrieve list of users from db, remove active user from list, sned back new list to data layer
    public void DeleteAccount(){}

    //a list that contains all the users
    public List<User> ListOfUsers{get;}

    // public bool findUser(string username)
    // {
    //     return ArrayOfUsers.Contains(username);
    // }

    public override bool Equals(object? o)
    {
        if(o == null || !(o is User))
        {
            return false;
        }
        
        return ((User)o).Username.Equals(username);
    }
}