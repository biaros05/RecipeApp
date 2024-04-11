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
}