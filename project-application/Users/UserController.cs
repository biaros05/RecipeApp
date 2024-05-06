using System.Collections.Generic;
namespace users;
using filtering;
using System.Net;
using recipes;
using System.Globalization;

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
        if (username == null)
        {
            throw new ArgumentNullException("Username cannot be null");
        }
        var context = RecipesContext.Instance;
        IQueryable<User> userQuery = context.RecipeManager_Users;
        filtering = new(userQuery);
        User result = filtering.FilterUsers(username);
        if (result != null)
        {
            throw new Exception("username already exists");
        }
        if (description == null)
        {

            User user1 = new User(username, password);
            RecipesContext.Instance.Add(user1);
        }
        else
        {
            User user1 = new User(username, password, description);
            RecipesContext.Instance.Add(user1);
        }
        RecipesContext.Instance.SaveChanges();
    }
    // make sure the user exists in the database, and the hashed password matches 
    // the hashed password of the username 
    // (interacts with data layer to retrieve list of users to perform authentication)
    // update ActiveUser
    public bool AuthenticateUser(string username, string password)
    {
        var context = RecipesContext.Instance;
        IQueryable<User> userQuery = context.RecipeManager_Users;
        filtering = new(userQuery);
        User result = filtering.FilterUsers(username);
        if (result == null)
        {
            throw new Exception("Username user doesnt exist");
        }
        if (Password.DoPasswordsMatch(password, result.Salt, result.HashPass))
        {
            ActiveUser = result;
            return true;
        }
        return false;
    }

    // retrieve list of users from db, remove active user from list, sned back new list to data layer
    //FIXME - need to use fluent API to on cascade delete
    public void DeleteAccount(string username, string password)
    {
        var context = RecipesContext.Instance;
        IQueryable<User> userQuery = context.RecipeManager_Users;
        bool result = AuthenticateUser(username, password);
        if (result)
        {
            filtering = new(userQuery);
            User user = filtering.FilterUsers(username);
            context.Remove(user);
            context.SaveChanges();
        }
    }

    public void UpdateUser(string username, string description, byte[] image, string hashpass)
    {
        var context = RecipesContext.Instance;

        int total = context.RecipeManager_Users
                    .Where(u => u.Username == username)
                    .Count();

        if (total != 0)
        {
            throw new Exception("this username is already taken");
        }

        User user = context.RecipeManager_Users
                    .Where(u => u.Username == this.ActiveUser.Username)
                    .First();

        this.ActiveUser.Description = description == null ? this.ActiveUser.Description : description;
        this.ActiveUser.Image = image == null ? this.ActiveUser.Image : image;
        this.ActiveUser.Username = username == null ? this.ActiveUser.Username : username;
        this.ActiveUser.HashPass = hashpass == null ? this.ActiveUser.HashPass : hashpass;

        user.Description = description == null ? this.ActiveUser.Description : description;
        user.Image = image == null ? this.ActiveUser.Image : image;
        user.Username = username == null ? this.ActiveUser.Username : username;
        user.HashPass = hashpass == null ? this.ActiveUser.HashPass : hashpass;

        context.Update(user);
        context.SaveChanges();
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
        set
        {
            _instance = value;
        }
    }


}