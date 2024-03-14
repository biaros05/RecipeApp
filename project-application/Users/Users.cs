using System.Collections.Generic;
namespace users;
using recipes;
public class Users 
{
    private User ActiveUser {get; set;}

    /// <summary>
    /// make sure the user exists in the database, and the hashed password matches 
    /// the hashed password of the username 
    /// (interacts with data layer to retrieve list of users to perform authentication)
    /// update ActiveUser
    /// </summary>
    public bool AuthenticateUser(string username, string password)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// retrieve list of users from db, remove active user from list, sned back new list to data layer
    /// </summary>
    public void DeleteAccount(){}
}