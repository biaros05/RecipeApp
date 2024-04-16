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
    private UserController userController;
    private User[] usersList = userController.ArrayOfUsers;
//USERlIST CONTAINS THE LIST OF ALL USERS
    private string username;
    private int id{get;set;}
    public string Username
    {
        get
        {
            return this.username;
        }
        set{
            foreach (User oneUser in usersList)
            {
                if(value.Equals(oneUser.Username))
                {
                    throw new Exception("this username is already in use");
                }
            }
            //or do i need =
            this.username.Equals(value);
        } 
    }
    public byte[]? Image {get;}
    public string? Description {get; set;}
    
    private Password Password;
    private byte[] Salt {get; set;}


    public override bool Equals(Object o)
    {
        throw new NotImplementedException();
    }

    // this constructor sets the username, hashes password and saves it
    public User(string username, Password password,string description, byte[] salt){
        
        this.Username=username;
        this.Password=password;
        this.Description=description;
        this.Salt=salt;
    }

    // performs the correct logic to update these fields and send changes to DB
    //shoudnt we be able to update each field seperatley?????
    public void UpdateFields(string newPassword, string newDescription, string newImage){}
    
    // removes profile picture provided one exists
    public void RemoveProfilePicture(){}

    // removes description provided one exists
    //new, shoudnt it update the desc and if user leave it empy it updates it as empty???
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