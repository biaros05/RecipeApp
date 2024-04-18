using System.Collections.Generic;
namespace users;
using filtering;
using System.Net;
using recipes;
public class UserController
{
    // currently logged on user
    public FilterByUsername? filtering;

    public static User? ActiveUser {get; set;}
    public static List<User> AllUsers{get;} = new();

    public UserController()
    {

    }
    
    // adds a new account (validates the input) --> should it take a user or params to create a new user individually?
    // public void CreateAccount(User newUser){}
    public void CreateAccount(string username, string password,string description)
    {
        filtering=new(AllUsers);
        User result=filtering.filterByUser(username);
        if (result!=null)
        {
            throw new Exception("username already exists");
        }
        if (description==null)
        {
            Password p=new Password(password);
            User user1 = new User(username,p);
            AllUsers.Add(user1);
        }
        else
        {
            Password p=new Password(password);
            User user1 = new User(username,p,description);
            AllUsers.Add(user1);
        }
    }
    // make sure the user exists in the database, and the hashed password matches 
    // the hashed password of the username 
    // (interacts with data layer to retrieve list of users to perform authentication)
    // update ActiveUser
    public bool AuthenticateUser(string username, string password)
    {
        filtering=new(AllUsers);
        User result=filtering.filterByUser(username);
        if (result==null)
        {
            throw new Exception("username user doesnt exist");
        }
        if(result.Password.DoPasswordsMatch(password))
                {
                    ActiveUser=result;
                    return true;
                }

        // foreach (User user in AllUsers)
        // {
        //     if(user.Username.Equals(username))
        //     {
        //         if(user.Password.DoPasswordsMatch(password))
        //         {
        //             ActiveUser=user;
        //             return true;
        //         }
        //     }
        // }
        return false;
    }

    // retrieve list of users from db, remove active user from list, sned back new list to data layer
    public void DeleteAccount(string username, string password){
            bool result=AuthenticateUser(username,password);
            if (result)
            {
                filtering=new(AllUsers);
                User user=filtering.filterByUser(username);
                AllUsers.Remove(user);
            }
        // foreach (User user in AllUsers)
        // {
        //     if(user.Username.Equals(username))
        //     {
        //         if(user.Password.DoPasswordsMatch(password)){
        //             AllUsers.Remove(user);
        //             Console.WriteLine("user sucessfully deleted");
        //         }
        //         // if(user.Password==password){
        //         //     AllUsers.Remove(user);
        //         //     Console.WriteLine("user sucessfully deleted");
        //         // }
                
        //         Console.WriteLine("user found but password was incorrect");
        //     }
        // }
        // Console.WriteLine("user doesn't exist");
    }

    //a list that contains all the users
    // public List<User> ListOfUsers{get;}

    // public bool findUser(string username)
    // {
    //     return ArrayOfUsers.Contains(username);
    // }

    // public override bool Equals(object? o)
    // {
    //     if(o == null || !(o is User))
    //     {
    //         return false;
    //     }
        
    //     return ((User)o).Username.Equals(username);
    // }
    //filter the username //to create a new user
    //filter the hashpassword //fot authenticate user
    //filter passsword and user // delete user


}