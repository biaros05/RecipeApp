using App.Views;
using System;
using System.Reactive;
using ReactiveUI;
using App.ViewModels;
using users;
using System.Collections.Generic;
using recipes;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.IO;

namespace App.ViewModels;

public class UserDetailsViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> Update { get; }
    public string DUsername{get;}

    public string DDescription{get;}

    public string DUserRecipes{get;}

    public string DUserFavoriteRecipes{get;}

    private Bitmap _imageDisplayed;

    public Bitmap ImageDisplayed
    {
    get => _imageDisplayed;
    set => this.RaiseAndSetIfChanged(ref _imageDisplayed, value);
    }

    private static readonly Bitmap PLACEHOLDER =
    // This shows an example of loading an image from the assets directory.
    new(AssetLoader.Open(new Uri("avares://App/Assets/default.jpg")));

    readonly User currentUser=UserController.Instance.ActiveUser;

    public string loopThroughList(List<Recipe> listOfRecipies,string msg)
    {
        string print="";
        if (listOfRecipies == null || listOfRecipies.Count == 0)
                {
                    print = msg;
                    return print;
                }
        foreach (Recipe recipe in listOfRecipies)
            {
                print+=$"{recipe} \n";
            }
            return print;
    }

    public string DisplayList()
    {
        string msg="Currently no created recipes";
        List<Recipe> listOfRecipes= this.currentUser.UserCreatedRecipies;
        string list=loopThroughList(listOfRecipes,msg);

        return list;
    }

    public string DisplayFavoritList()
    {   
        string msg="Currently no favorited recipes";
        List<Recipe> listOfRecipes=this.currentUser.UserFavoriteRecipies.ToList();
        string list=loopThroughList(listOfRecipes,msg);

        return list;
    }

    
    public UserDetailsViewModel()
    {
        Update = ReactiveCommand.Create(() =>
        {
    // we know both values are not null, because of `areBothFilledIn`
        // UserController.Instance.UpdateUser();
        });
    // .CurrentlyLoggedInUser.DisplayName
    // User u=UserController.Instance.ActiveUser;
        DUsername=$"Username: {this.currentUser.Username}";
        DDescription=$"Description: {this.currentUser.Description}";
        DUserRecipes=DisplayList();
        DUserFavoriteRecipes=DisplayFavoritList();
        if (currentUser.Image==null)
        {
            ImageDisplayed=PLACEHOLDER;
        }
        
        else{
            ImageDisplayed= new(new MemoryStream(UserController.Instance.ActiveUser.Image));
        }    
    }
}