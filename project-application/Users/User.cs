using System.Collections.Generic;
using System.Reflection;
using filtering;
namespace users;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using recipes;

public class User
{
    public User(){}
    public int UserId {get; set;}

    public string HashPass;

    public byte[] Salt;

    [InverseProperty("Owner")]
    public List<Recipe> UserCreatedRecipies { get; set; }

    public ICollection<Recipe> UserFavoriteRecipies { get; set; }
    private string username;
    public string Username
    {
        get
        {
            return this.username;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("password cannot be empty");
            }
            if (value.Length < 5 || value.Length > 25)
            {
                throw new ArgumentException("username must be between 5-25 characters");
            }
            this.username = value;
        }
    }
    public byte[]? Image { get; set; }
    private string? description;
    public string? Description
    {
        get
        {
            return this.description;
        }

        set
        {
            if (value?.Length > 500)
            {
                throw new Exception("description is too long. needs to be less than 500 charcters");
            }
            this.description = value;
        }
    }

    public override bool Equals(object? o)
    {
        if (o == null || !(o is User))
        {
            return false;
        }

        return ((User)o).Username.Equals(this.Username);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Username);
    }

    // this constructor sets the username, hashes password and saves it
    //, byte[] salt
    public User(string username, string password, string description)
    {
        this.Username = username;
        this.Salt=Password.GenerateSalt();
        this.HashPass=Password.HashPassword(this.Salt,password);

        this.Description = description;
        UserCreatedRecipies = new();
        UserFavoriteRecipies = new List<Recipe>();
        //Image need to be here as a byte[]
    }

    public User(string username, string password)
    {
        this.Username = username;
        this.Salt=Password.GenerateSalt();
        this.HashPass=Password.HashPassword(this.Salt,password);

        this.Description = null;
        UserCreatedRecipies = new();
        UserFavoriteRecipies = new List<Recipe>();
        //Image need to be here as a byte[]
        //asign default pic

    }

    public void UpdateUsername(string newUsername)
    {
        using var context= new RecipesContext();

        User user= context.RecipeManager_Users
                    .Where(u=>u.username==newUsername)
                    .First();

        if (newUsername.Length < 5 || newUsername.Length > 25 || newUsername == null)
        {
            throw new Exception("username doesnt meet requirements");
        }

        user.Username = newUsername;
        context.SaveChanges();
    }

    public void UpdatePassword(string newPass)
    {
        using var context= new RecipesContext();

        if (newPass.Length < 5 || newPass.Length > 50)
        {
            throw new Exception("password doesnt meet requirements");
        }
        User user= context.RecipeManager_Users
                    .Where(u=>u.username==this.Username)
                    .First();

        user.HashPass=Password.HashPassword(this.Salt,newPass);
        context.SaveChanges();
    }
    public void UpdateFields(string newDescription, byte[] newImage)
    {
        using var context= new RecipesContext();
        User user= context.RecipeManager_Users
                    .Where(u=>u.username==this.Username)
                    .First();

        user.Description = newDescription;
        user.Image = newImage;
        context.SaveChanges();
    }

    // removes profile picture provided one exists
    public void RemoveProfilePicture()
    {
        using var context= new RecipesContext();
        User user= context.RecipeManager_Users
                    .Where(u=>u.username==this.Username)
                    .First();

        if (user.Image == null)
        {
            throw new Exception("no picture to remove");
        }
        user.Image = null;
        context.SaveChanges();
    }

    // removes description provided one exists
    public void RemoveDescription()
    {
        using var context= new RecipesContext();
        User user= context.RecipeManager_Users
                    .Where(u=>u.username==this.Username)
                    .First();

        if (Description == null)
        {
            throw new Exception("no description to remove");
        }
        
        user.Description = null;
        context.SaveChanges();
    }

    // take recipe, retrieve the list of recipes for user, add that recipe, send back to data layer
    public void AddToFavourites(Recipe recipe)
    {
        using var context= new RecipesContext();
        User user= context.RecipeManager_Users
                    .Where(u=>u.username==this.Username)
                    .First();

        if (recipe == null)
        {
            throw new ArgumentNullException("Recipe cannot be null");
        }
        if (user.UserFavoriteRecipies.Contains(recipe))
        {
            throw new ArgumentException("This recipe has already been added to favourites");
        }
        user.UserFavoriteRecipies.Add(recipe);
        context.SaveChanges();
    }

    // take recipe, retrieve the list of recipes for user, remove that recipe, send back to data layer
    public void RemoveFromFavourites(Recipe recipe)
    {
        using var context= new RecipesContext();
        User user= context.RecipeManager_Users
                    .Where(u=>u.username==this.Username)
                    .First();
        
        if (recipe == null)
        {
            throw new ArgumentNullException("Recipe cannot be null");
        }
        if (!user.UserFavoriteRecipies.Contains(recipe))
        {
            throw new ArgumentException("This recipe was never added to your favourites");
        }
        user.UserFavoriteRecipies.Remove(recipe);
        context.SaveChanges();
    }

    public override string ToString()
    {
        return this.Username + this.Image + this.Description;
    }

    // ad hash and salt to constructor


}