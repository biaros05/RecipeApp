using System.Collections.Generic;
namespace users;
using recipes;
public class Users 
{
    // currently logged on user
    public User ActiveUser {get; set;}
    
    // adds a new account (validates the input) --> should it take a user or params to create a new user individually?
    public void CreateAccount(User newUser){}

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
    public string[] ArrayOfUsers{get;}

    public bool findUser(string username)
    {
        return ArrayOfUsers.Contains(username);
    }
}