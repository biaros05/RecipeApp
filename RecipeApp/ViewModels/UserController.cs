using System.Collections.Generic;
namespace users;
using filtering;
using System.Net;
using recipes;
using System.Globalization;
using System;
using System.Linq;

public class UserController
{
    // currently logged on user
    public FilterByUsername? filtering;
    public int MIN_PASSWORD_LENGTH=5;

    public int MAX_PASSWORD_LENGTH=25;
    public User? ActiveUser { get; set; }
    public List<User> AllUsers { get; } = new();

    /// <summary>
    /// this will make sure the given username does not already exist and is not null
    /// and will create a user
    /// </summary>
    /// <param name="username">new username</param>
    /// <param name="password">new password</param>
    /// <param name="description">user's description</param>
    public User CreateAccount(string username, string password, string description)
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
            RecipesContext.Instance.SaveChanges();
            return user1;
        }
        // else
        // {
        User user2 = new User(username, password, description);
        RecipesContext.Instance.Add(user2);
        // }
        RecipesContext.Instance.SaveChanges();
        return user2;
    }

    /// <summary>
    /// this method will make sure the user exists in the database
    /// by verifying the username existing, and then matching the 
    /// passed password with the user's password.
    /// </summary>
    /// <param name="username">username to login with</param>
    /// <param name="password">the given password to be validated</param>
    /// <returns></returns>
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

    /// <summary>
    /// this will take in a username for the user to be deleted, 
    /// and will make sure the given password matches the user
    /// </summary>
    /// <param name="username">username for user to be deleted</param>
    /// <param name="password">password of said user (must match)</param>
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

    public void UpdateUser(string? username, string? description, byte[]? image, string? hashpass)
    {
        var context = RecipesContext.Instance;

        int total = context.RecipeManager_Users
                    .Where(u => u.Username == username)
                    .Count();

        // if (total > 1)
        // {
        //     throw new Exception("this username is already taken");
        // }
        if (username == ActiveUser.Username || total == 0)
        {
            User user = context.RecipeManager_Users
                    .Where(u => u.Username == this.ActiveUser.Username)
                    .First();

        this.ActiveUser.Description = description == null ? this.ActiveUser.Description : description;
        this.ActiveUser.Image = image;
        this.ActiveUser.Username = username == null ? this.ActiveUser.Username : username;
        this.ActiveUser.HashPass = hashpass == null ? this.ActiveUser.HashPass : hashpass;

        user.Description = description == null ? this.ActiveUser.Description : description;
        user.Image = image;
        user.Username = username == null ? this.ActiveUser.Username : username;
        user.HashPass = hashpass == null ? this.ActiveUser.HashPass : hashpass;

        context.Update(user);
        context.SaveChanges();
        }
        else{
            throw new Exception("this username is already taken");
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
        set
        {
            _instance = value;
        }
    }

    public void Logout()
    {
        this.ActiveUser = null;
    }

}