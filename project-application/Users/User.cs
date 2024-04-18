using System.Collections.Generic;
using System.Reflection;
namespace users;

using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using recipes;

public class User
{
//store salt(from pwd to user)
//store img as a array of bytes
//add user id only visible in db(private)

//ignore salt just take in the user and pwd
//access the properties like explained in clas and ex on stack
    [InverseProperty("UserCreatedRecipies")]
    public List<Recipe> UserCreatedRecipies{get;set;}

    [InverseProperty("UserFavoriteRecipies")]
    public List<Recipe> UserFavoriteRecipies{get;set;}
    private string username;
    private int Id{get;set;}
    public string Username
    {
        get
        {
            return this.username;
        }
        set{
            // foreach (User oneUser in usersList)
            // {
            //     if(value.Equals(oneUser.Username))
            //     {
            //         throw new Exception("this username is already in use");
            //     }
            // }
            this.username=value;
        } 
    }
    public byte[]? Image {get;set;}
    public string? Description {get; set;}
    
    public Password Password;
    // private byte[] Salt {get; set;}


    public override bool Equals(object? o)
    {
        if(o == null || !(o is User))
        {
            return false;
        }
        
        return ((User)o).Username.Equals(this.Username);
    }

    // this constructor sets the username, hashes password and saves it
    //, byte[] salt
    public User( string username, Password password,string description){
        this.Username=username;
        this.Password=password;
        this.Description=description;
        // this.Salt=salt; need to set salt
    }

    // performs the correct logic to update these fields and send changes to DB
    //shoudnt we be able to update each field seperatley?????
    public void UpdatePassword(Password newPass)
    {
        Password=newPass;
    }
    public void UpdateFields( string newDescription, byte[] newImage){
        this.Description=newDescription;
        this.Image=newImage;
    }
        
    // removes profile picture provided one exists
    public void RemoveProfilePicture(){
        this.Image=null;
    }

    // removes description provided one exists
    public void RemoveDescription(){
        this.Description=null;
    }

    // take recipe, retrieve the list of recipes for user, add that recipe, send back to data layer
    public void AddToFavourites(Recipe recipe){
        UserFavoriteRecipies.Add(recipe);
    }

    // take recipe, retrieve the list of recipes for user, remove that recipe, send back to data layer
    public void RemoveFromFavourites(Recipe recipe){
        UserFavoriteRecipies.Remove(recipe);
    }

    // interacts with data layer to retrieve the recipes by that user
}