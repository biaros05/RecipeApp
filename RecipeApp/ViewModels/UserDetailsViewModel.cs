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

namespace App.ViewModels;

public class UserDetailsViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> Update { get; }
    public string DUsername{get;}

    public string DDescription{get;}

    public string DUserRecipes{get;}

    public string DUserFavoriteRecipes{get;}

    readonly User currentUser=UserController.Instance.ActiveUser;

    public string DisplayList()
    {
        string print="";
        try
        {
            foreach (Recipe recipe in this.currentUser.UserCreatedRecipies)
            {
                print+=$"{recipe} /n";
            }
            return print;
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine("A NullReferenceException was caught!");
            Console.WriteLine($"Exception message: {ex.Message}");
            if (ex.Message.Contains("Object reference not set to an instance of an object"))
            {
                print = "No recipes created yet.";
            }
        }
        return print;
    }

    public string DisplayFavoritList()
    {
        string print="";
        try
        {
            foreach (Recipe recipe in this.currentUser.UserFavoriteRecipies)
            {
                if (this.currentUser.UserFavoriteRecipies == null || this.currentUser.UserFavoriteRecipies.Count == 0)
                {
                    print = "No recipes favorited yet.";
                    return print;
                }
                print+=$"{recipe} /n";
            }
            return print;
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine("A NullReferenceException was caught!");
            Console.WriteLine($"Exception message: {ex.Message}");
            if (ex.Message.Contains("Object reference not set to an instance of an object"))
            {
                print = "No recipes favorited yet.";
            }
        }
        return print;
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
    
    }
}