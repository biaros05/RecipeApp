using System.Collections.Generic;
namespace users;
using filtering;
using System.Net;
using recipes;
public class UserController
{
    // currently logged on user
    public FilterByUsername? filtering;

    public User? ActiveUser { get; set; }
    public List<User> AllUsers { get; } = new();

    // adds a new account (validates the input) --> should it take a user or params to create a new user individually?
    // public void CreateAccount(User newUser){}
    public void CreateAccount(string username, string password, string description)
    {
        filtering = new(AllUsers);
        User result = filtering.FilterUsers(username);
        if (result != null)
        {
            throw new Exception("username already exists");
        }
        if (description == null)
        {
            
            User user1 = new User(username, password);
            AllUsers.Add(user1);
        }
        else
        {
            User user1 = new User(username, password, description);
            AllUsers.Add(user1);
        }
    }
    // make sure the user exists in the database, and the hashed password matches 
    // the hashed password of the username 
    // (interacts with data layer to retrieve list of users to perform authentication)
    // update ActiveUser
    public bool AuthenticateUser(string username, string password)
    {
        filtering = new(AllUsers);
        User result = filtering.FilterUsers(username);
        if (result == null)
        {
            throw new Exception("Username user doesnt exist");
        }
        if (Password.DoPasswordsMatch(password,result.Salt,result.HashPass))
        {
            ActiveUser = result;
            return true;
        }
        return false;
    }

    // retrieve list of users from db, remove active user from list, sned back new list to data layer
    public void DeleteAccount(string username, string password)
    {
        bool result = AuthenticateUser(username, password);
        if (result)
        {
            filtering = new(AllUsers);
            User user = filtering.FilterUsers(username);
            AllUsers.Remove(user);
        }
    }

    private static UserController? _instance;

        private UserController() { }
        public static UserController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserController();
                }
                return _instance;
            }
        }


}