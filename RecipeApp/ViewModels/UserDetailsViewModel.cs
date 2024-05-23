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
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    } 
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

    private string? _password;
    public string? Password{
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
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

    public ReactiveCommand<Unit, bool> DeleteUser;

    
    public UserDetailsViewModel()
    {
        Update = ReactiveCommand.Create(() =>
        {

        });

        DUsername=$"Username: {this.currentUser.Username}";
        DDescription=$"Description: {this.currentUser.Description}";
        DUserRecipes=DisplayList();
        DUserFavoriteRecipes=DisplayFavoritList();

        if (currentUser.Image==null)
        {
            ImageDisplayed=PLACEHOLDER;
        }
        
        else
        {
            ImageDisplayed= new(new MemoryStream(UserController.Instance.ActiveUser.Image));
        }

        IObservable<bool> areFieldsFilled = this.WhenAnyValue(
        recipeViewModel => recipeViewModel.Password,
        (name) =>
            !(string.IsNullOrEmpty(name))
        );

        DeleteUser = ReactiveCommand.Create(() =>
        {
            if (UserController.Instance.AuthenticateUser(UserController.Instance.ActiveUser!.Username, Password!))
            {
                UserController.Instance.DeleteAccount(UserController.Instance.ActiveUser!.Username, Password!);
                return true;
            }
            else
            {
                ErrorMessage = "Your credentials are incorrect";
                return false;
            }
        }, areFieldsFilled);
    }
}