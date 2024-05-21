
using System.Collections.Generic;
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
    public string Greeting{get;}

    public User CurrentUser {get; set; }

    public List<Recipe> UserRecipes {get; set;}

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
        UserRecipes = CurrentUser.UserCreatedRecipies;

        RemoveRecipe = ReactiveCommand.Create<Recipe>((r) =>
        {
            RecipeController.DeleteRecipe(r);
        });

    }
}