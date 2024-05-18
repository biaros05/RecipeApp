using App.Views;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using recipes;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    
    public void NavigateToEditInstructions(Recipe? recipe = null)
    {
        RecipeInstructionEditViewModel instructionViewModel = new(recipe);
        instructionViewModel.Save.Subscribe(newRecipe => 
        {
            NavigateToEditRecipe(newRecipe);
        });
        instructionViewModel.Cancel.Subscribe(oldRecipe =>
        {
            NavigateToEditRecipe(oldRecipe);
        });
        ContentViewModel = instructionViewModel;
    }

    public void NavigateToEditIngredients(Recipe? recipe = null)
    {
        RecipeIngredientEditViewModel ingredientEditView = new(recipe);
        ingredientEditView.Save.Subscribe(r => NavigateToEditRecipe(r));
        ingredientEditView.Cancel.Subscribe(r => NavigateToEditRecipe(r));
        ContentViewModel = ingredientEditView;
    }

    public void NavigateToEditRecipe(Recipe? recipe = null)
    {
        RecipeEditViewModel recipeViewModel = new RecipeEditViewModel(recipe);
        recipeViewModel.InstructionButton.Subscribe(r => {
            NavigateToEditInstructions(r);
        });

        recipeViewModel.IngredientButton.Subscribe(r => {
            NavigateToEditIngredients(r);
        });

        recipeViewModel.TagButton.Subscribe(r => {
            //NavigateToEditTags(r);
        });

        // ADD FOR SAVE AND CANCEL HERE TOO
        ContentViewModel = recipeViewModel;
    }

    // TO BE CHANGED
    public MainWindowViewModel()
    {
        NavigateToEditRecipe();
    }
}