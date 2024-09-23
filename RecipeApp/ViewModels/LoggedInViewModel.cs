
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using App.ViewModels;
using ReactiveUI;
using recipes;
using users;
namespace App.ViewModels;

public class LoggedInViewModel: ViewModelBase
{
    public ReactiveCommand<Unit, Unit> Logout { get; }
    public ReactiveCommand<Recipe, Unit> RemoveRecipe {get;}
    public ReactiveCommand<Unit, Recipe?> CreateRecipe {get;}

    public ReactiveCommand<Recipe, Recipe> EditRecipe {get;}
    public string Greeting{get;}

    public User CurrentUser {get; set; }

    public ObservableCollection<Recipe> UserRecipes {get; set;}

    public LoggedInViewModel()
    {
        Logout = ReactiveCommand.Create(() =>
        {
            // we know both values are not null, because of `areBothFilledIn`
            UserController.Instance.Logout();
        });

        // .CurrentlyLoggedInUser.DisplayName
        User u=UserController.Instance.ActiveUser!;
        Greeting=$"hello {u.Username}";

        CurrentUser = UserController.Instance.ActiveUser!;
        UserRecipes = new(CurrentUser.UserCreatedRecipies);

        RemoveRecipe = ReactiveCommand.Create<Recipe>((r) =>
        {
            RecipeController.DeleteRecipe(r);
            UserRecipes.Remove(r);
        });

        CreateRecipe =  ReactiveCommand.Create<Recipe>(()=> null );
        EditRecipe = ReactiveCommand.Create<Recipe, Recipe>((r) => {
            return r;
        });
    }
}