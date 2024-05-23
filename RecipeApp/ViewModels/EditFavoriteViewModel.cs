using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using App.ViewModels;
using DynamicData.Kernel;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;
public class EditFavoriteViewModel : ViewModelBase
{
    private ObservableCollection<Recipe> recipeList;
    public ObservableCollection<Recipe> RecipeList
    {
        get => recipeList;
        set => this.RaiseAndSetIfChanged(ref recipeList, value);
    }
    private Recipe? _selectedRecipe;
    public Recipe? SelectedRecipe
    {
        get => _selectedRecipe;
        set => this.RaiseAndSetIfChanged(ref _selectedRecipe, value);
    }
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

    public ReactiveCommand<Unit, Recipe?> ViewRecipeCommand { get; }

    public EditFavoriteViewModel()
    {

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        List<Recipe> loadRecipeList = RecipeController.Instance.AllRecipes;
        recipeList = new(UserController.Instance.ActiveUser.UserFavoriteRecipies);
        if (recipeList == null || recipeList.Count == 0)
        {
            ErrorMessage = "You do not have any favorite recipes";
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        ViewRecipeCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedRecipe == null)
            {
                ErrorMessage = "Select a recipe";
                return null;
            }
            return SelectedRecipe;
        });

        RemoveCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedRecipe == null)
            {
                ErrorMessage = "Select a recipe";
            }
            else
            {
                if (UserController.Instance.ActiveUser.UserFavoriteRecipies.Contains(SelectedRecipe))
                {
                    UserController.Instance.ActiveUser.RemoveFromFavourites(SelectedRecipe);
                    ErrorMessage = "Removed from favorites";
                }
                else
                {
                    UserController.Instance.ActiveUser.AddToFavourites(SelectedRecipe);
                }
                RecipeList = new(UserController.Instance.ActiveUser.UserFavoriteRecipies);
            }
        });
    }
}