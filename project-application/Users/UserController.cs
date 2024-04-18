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
        foreach (User user in AllUsers)
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
        int total=AllUsers.Count+1;
        Password p=new Password(password);
        User user1 = new User(total,username,p,description);
        AllUsers.Add(user1);
    }
    // make sure the user exists in the database, and the hashed password matches 
    // the hashed password of the username 
    // (interacts with data layer to retrieve list of users to perform authentication)
    // update ActiveUser
    public bool AuthenticateUser(string username, string password)
    {
        foreach (User user in AllUsers)
        {
            if(user.Username.Equals(username))
            {
                if(user.Password.Equals(password))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // retrieve list of users from db, remove active user from list, sned back new list to data layer
    public void DeleteAccount(string username, string password){
        
        foreach (User user in AllUsers)
        {
            if(user.Username.Equals(username))
            {
                if(user.Password.Equals(password)){
                    AllUsers.Remove(user);
                    Console.WriteLine("user sucessfully deleted");
                }
                
                Console.WriteLine("user found but password was incorrect");
            }
        }
        Console.WriteLine("user doesn't exist");
    }

    //a list that contains all the users
    // public List<User> ListOfUsers{get;}

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