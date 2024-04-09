using System.Collections.Generic;
using System.Reflection;
namespace users;
using recipes;
public class User
{
//store salt(from pwd to user)
//store img as a array of bytes
//add user id only visible in db(private)


//ignore salt just take in the user and pwd


    private int id{get;set;}
    public string Username {get; set;}
    public byte[]? Image {get;}
    public string? Description {get; set;}
    //private Password Password {get; set;}
    private string Password{get;set;}
    private byte[] Salt {get; set;}

    public Users allUsers;

    public override bool Equals(Object o)
    {
        throw new NotImplementedException();
    }

    // this constructor sets the username, hashes password and saves it
    public User(string username, string password){
        if((password.Length>50 || password.Length<5) || allUsers.findUser(username) )
        {
           throw new Exception("smt g"); 
        }
        this.Username=username;
        this.Password=password;
    }

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