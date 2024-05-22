using System;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using recipes;
using filtering;
using users;
using Microsoft.EntityFrameworkCore;
using App.ViewModels;
using recipes;
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

    public static bool EditingRecipe;
    
    public void NavigateToEditInstructions(Recipe? recipe = null)
    {
        RecipeInstructionEditViewModel instructionViewModel = new(recipe);
        instructionViewModel.Save.Subscribe(r =>{
            if (EditingRecipe)
                NavigateToEditRecipe(r);
            else
                NavigateToCreateRecipe(r);
        });
        instructionViewModel.Cancel.Subscribe(r =>{
            if (EditingRecipe)
                NavigateToEditRecipe(r);
            else
                NavigateToCreateRecipe(r);
        });
        ContentViewModel = instructionViewModel;
    }

    public void NavigateToEditIngredients(Recipe? recipe = null)
    {
        RecipeIngredientEditViewModel ingredientEditView = new(recipe);
        ingredientEditView.Save.Subscribe(r => {
            if (EditingRecipe)
                NavigateToEditRecipe(r);
            else
                NavigateToCreateRecipe(r);
        });
        ingredientEditView.Cancel.Subscribe(r => {
            if (EditingRecipe)
                NavigateToEditRecipe(r);
            else
                NavigateToCreateRecipe(r);
        });
        ContentViewModel = ingredientEditView;
    }

    public void NavigateToCreateRecipe(Recipe? recipe = null)
    {
        EditingRecipe = false;
        RecipeCreateViewModel recipeViewModel = new RecipeCreateViewModel(recipe);
        recipeViewModel.InstructionButton.Subscribe(r => {
            NavigateToEditInstructions(r);
        });

        recipeViewModel.IngredientButton.Subscribe(r => {
            NavigateToEditIngredients(r);
        });

        recipeViewModel.TagButton.Subscribe(r => {
            NavigateToEditTags(r);
        });

        // ADD FOR SAVE AND CANCEL HERE TOO

        recipeViewModel.Save.Subscribe(r => 
        {
            NavigateToLoggedIn();
        });

        ContentViewModel = recipeViewModel;
    }

    public void NavigateToEditRecipe(Recipe recipe)
    {
        EditingRecipe = true;
        RecipeEditViewModel recipeViewModel = new(recipe);
        recipeViewModel.InstructionButton.Subscribe(r => {
            NavigateToEditInstructions(r);
        });

        recipeViewModel.IngredientButton.Subscribe(r => {
            NavigateToEditIngredients(r);
        });

        recipeViewModel.TagButton.Subscribe(r => {
            NavigateToEditTags(r);
        });

        // ADD FOR SAVE AND CANCEL HERE TOO
        recipeViewModel.Save.Subscribe((r) => 
        {
            NavigateToLoggedIn();
        });

        ContentViewModel = recipeViewModel;
    }

    public void NavigateToEditTags(Recipe? recipe = null)
    {
        RecipeTagsEditViewModel recipeTagsEditView = new(recipe);
        recipeTagsEditView.Cancel.Subscribe((r) => 
        {
            if (EditingRecipe)
                NavigateToEditRecipe(r);
            else
                NavigateToCreateRecipe(r);
        });
        recipeTagsEditView.Save.Subscribe((r) => 
        {
            if (EditingRecipe)
                NavigateToEditRecipe(r);
            else
                NavigateToCreateRecipe(r);
        });
        ContentViewModel = recipeTagsEditView;
    }

    // TO BE CHANGED
    public MainWindowViewModel()
    {
        EditingRecipe = false;
        ContentViewModel = new WelcomeViewModel();

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
        viewModel.CreateRecipe.Subscribe(_ => NavigateToCreateRecipe(null));
        viewModel.EditRecipe.Subscribe(r => NavigateToEditRecipe(r));

        ContentViewModel = viewModel;
    }


}