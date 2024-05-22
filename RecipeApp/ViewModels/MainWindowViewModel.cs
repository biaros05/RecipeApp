using System;
using ReactiveUI;
using recipes;
using System.Reactive.Linq;
using App.Views;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public MainWindowViewModel()
    {
        _contentViewModel = new WelcomeViewModel();
    }
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public void NavigateToRecipeList()
    {
        RecipeListViewModel viewModel = new();

        viewModel.ViewRecipeCommand.Subscribe(recipe =>
        {
            if (recipe != null)
            {
                NavigateToRecipe(recipe);
            }
        });
        ContentViewModel = viewModel;
    }
    public void NavigateToWelcome()
    {
        ContentViewModel = new WelcomeViewModel();
    }

    public void NavigateToRegister()
    {
        RegisterViewModel viewModel = new();

        viewModel.Register.Subscribe(user =>
        {
            if (user != null)
            {
                NavigateToWelcome();
            }
        });

        ContentViewModel = viewModel;
    }

    public void NavigateToLogin()
    {
        LoginViewModel viewModel = new();

        viewModel.Login.Subscribe(user =>
        {
            if (user != null)
            {
                NavigateToLoggedIn();
            }
        });

        ContentViewModel = viewModel;
    }

    public void NavigateToRecipe(Recipe recipe)
    {
        ContentViewModel = new RecipeViewModel(recipe);
    }

    public void NavigateToLoggedIn()
    {
        LoggedInViewModel viewModel = new();

        viewModel.Logout.Subscribe(_ => NavigateToWelcome());

        ContentViewModel = viewModel;
    }

    public void NavigateToFavorites()
    {
        EditFavoriteViewModel viewModel = new();

        viewModel.ViewRecipeCommand.Subscribe(recipe =>
        {
            if (recipe != null)
            {
                NavigateToRecipe(recipe);
            }
        });

        ContentViewModel = viewModel;
    }

}