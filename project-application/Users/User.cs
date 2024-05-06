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
    private byte[]? image;
    private string hashPass;
    public User() { }
    public int UserId { get; set; }

    public string HashPass
    {
        get
        {
            return this.hashPass;
        }
        set
        {
            if (value.Length < 5 || value.Length > 50)
            {
                throw new Exception("password doesnt meet requirements");
            }
            this.hashPass = Password.HashPassword(this.Salt, value);
        }
    }

    public byte[] Salt { get; set; }


    [InverseProperty("Owner")]
    public List<Recipe> UserCreatedRecipies { get; set; }

    public ICollection<Recipe> UserFavoriteRecipies { get; set; } = new List<Recipe>();
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
    public byte[]? Image
    {
        get
        {
            return this.image;
        }
        set
        {
            if (value == null)
            {
                this.image = null;
            }
            this.image = value;
        }
    }
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
    public User(string username, string password, string description = null, byte[] image = null)
    {
        this.Username = username;
        this.Salt = Password.GenerateSalt();
        this.HashPass = password;
        this.Image = image;


        this.Description = description;
        UserCreatedRecipies = new();
        UserFavoriteRecipies = new List<Recipe>();
    }

    public void AddToFavourites(Recipe recipe)
    {
        var context = RecipesContext.Instance;
        User user = context.RecipeManager_Users.ToList()
                    .Where(u => u.username.Equals(this.Username))
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
        context.Update(user);
        context.SaveChanges();
    }

    // take recipe, retrieve the list of recipes for user, remove that recipe, send back to data layer
    public void RemoveFromFavourites(Recipe recipe)
    {
        var context = RecipesContext.Instance;
        User user = context.RecipeManager_Users.ToList()
                    .Where(u => u.username.Equals(this.Username))
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
        context.Update(user);
        context.SaveChanges();
    }

    public override string ToString()
    {
        return this.Username + this.Image + this.Description;
    }
}