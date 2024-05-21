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

    public string loopThroughList(List<Recipe> listOfRecipies)
    {
        string print="";
        if (listOfRecipies == null || listOfRecipies.Count == 0)
                {
                    print = "No recipes favorited yet.";
                    return print;
                }
        foreach (Recipe recipe in listOfRecipies)
            {
                print+=$"{recipe} /n";
            }
            return print;
    }

    public string DisplayList()
    {
        List<Recipe> listOfRecipes= this.currentUser.UserCreatedRecipies;
        string list=loopThroughList(listOfRecipes);

        return list;
    }

    public string DisplayFavoritList()
    {
        List<Recipe> listOfRecipes=this.currentUser.UserFavoriteRecipies.ToList();
        string list=loopThroughList(listOfRecipes);

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
    
    }
}