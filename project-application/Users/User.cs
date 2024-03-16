using System.Collections.Generic;
using System.Reflection;
namespace users;
using recipes;
public class User
{
    public string Username {get; set;}
    public string? Image {get;}
    public string? Description {get; set;}
    private Password Password {get; set;}
    
    public override bool Equals(Object o)
    {
        throw new NotImplementedException();
    }

    // this constructor sets the username, hashes password and saves it
    public User(string username, string password){}

    // performs the correct logic to update these fields and send changes to DB
    public void UpdateFields(string newPassword, string newDescription, string newImage){}
    
    // removes profile picture provided one exists
    public void RemoveProfilePicture(){}

    // removes description provided one exists
    public void RemoveDescription(){}

    // take recipe, retrieve the list of recipes for user, add that recipe, send back to data layer
    public void AddToFavourites(Recipe recipe){}

    // take recipe, retrieve the list of recipes for user, remove that recipe, send back to data layer
    public void RemoveFromFavourites(Recipe recipe){}

    // interacts with data layer to retrieve the recipes by that user
    public List<Recipe> GetRecipesCreated()
    {
        throw new NotImplementedException();
    }

    // interacts with data layer to retrieve the favourites for that user
    public List<Recipe> GetFavourites()
    {
        throw new NotImplementedException();
    }

}