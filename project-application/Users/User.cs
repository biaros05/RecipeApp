using System.Collections.Generic;
using System.Reflection;
namespace users;
using recipes;
public class User
{
    private string Username {get; set;}
    private string? Image {get;}
    private string? Description {get; set;}
    private string HashedPassword {get; set;}
    private byte[] Salt {get; set;}
    
    public override bool Equals(Object o)
    {
        throw new NotImplementedException();
    }

    // this constructor sets the username, hashes password and saves it
    public User(string username, string password){}

    // hashes password, saves, and sends to data layer
    public void UpdatePassword(string newPassword, string newDescription, string newImage){}

    // take recipe, retrieve the list of recipes for user, add that recipe, send back to data layer
    public void AddToFavourites(Recipe recipe){}

    // take recipe, retrieve the list of recipes for user, remove that recipe, send back to data layer
    public void RemoveFromFavourites(Recipe recipe){}

    // interacts with data layer to retrieve the recipes by that user
    private List<Recipe> GetRecipesCreated()
    {
        throw new NotImplementedException();
    }

    // interacts with data layer to retrieve the favourites for that user
    private List<Recipe> GetFavourites()
    {
        throw new NotImplementedException();
    }
    // implements algo for hashing
    private string HashPassword()
    {
        throw new NotImplementedException();
    }

}