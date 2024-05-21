using App.Views;
using ReactiveUI;
using recipes;
using System;
using System.Reactive.Linq;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public MainWindowViewModel()
    {
        _contentViewModel = new();
        NavigateToRecipeList();
    }

    public void NavigateToRecipeList()
    {
        RecipeListViewModel viewModel = new(RecipeController.Instance.AllRecipes);

        viewModel.ViewRecipeCommand.Subscribe(recipe =>
        {
            if (recipe != null)
            {
                NavigateToRecipe(recipe);
            }
        });

        ContentViewModel = viewModel;
    }

    public void NavigateToRecipe(Recipe recipe)
    {
        ContentViewModel = new RecipeViewModel(recipe);
    }

    public void NavigateToWelcome()
    {
        //NOTE - back to welcome page
        ContentViewModel = new MainWindowViewModel();
    }
}