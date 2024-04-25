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

    //[InverseProperty("UserFavourite")]
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
        if (newUsername.Length < 5 || newUsername.Length > 25 || newUsername == null)
        {
            throw new Exception("username doesnt meet requirements");
        }
        Username = newUsername;
    }

    public void UpdatePassword(string newPass)
    {
        if (newPass.Length < 5 || newPass.Length > 50)
        {
            throw new Exception("password doesnt meet requirements");
        }

        this.HashPass=Password.HashPassword(this.Salt,newPass);    
    }
    public void UpdateFields(string newDescription, byte[] newImage)
    {
        this.Description = newDescription;
        this.Image = newImage;
    }

    // removes profile picture provided one exists
    public void RemoveProfilePicture()
    {
        if (Image == null)
        {
            throw new Exception("no picture to remove");
        }
        this.Image = null;
    }

    // removes description provided one exists
    public void RemoveDescription()
    {
        if (Description == null)
        {
            throw new Exception("no description to remove");
        }
        this.Description = null;
    }

    // take recipe, retrieve the list of recipes for user, add that recipe, send back to data layer
    public void AddToFavourites(Recipe recipe)
    {
        if (recipe == null)
        {
            throw new ArgumentNullException("Recipe cannot be null");
        }
        if (this.UserFavoriteRecipies.Contains(recipe))
        {
            throw new ArgumentException("This recipe has already been added to favourites");
        }
        UserFavoriteRecipies.Add(recipe);
    }

    // take recipe, retrieve the list of recipes for user, remove that recipe, send back to data layer
    public void RemoveFromFavourites(Recipe recipe)
    {
        if (recipe == null)
        {
            throw new ArgumentNullException("Recipe cannot be null");
        }
        if (!this.UserFavoriteRecipies.Contains(recipe))
        {
            throw new ArgumentException("This recipe was never added to your favourites");
        }
        UserFavoriteRecipies.Remove(recipe);
    }

    public override string ToString()
    {
        return this.Username + this.Image + this.Description;
    }

    // ad hash and salt to constructor


}