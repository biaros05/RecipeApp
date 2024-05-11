using App.Views;
using ReactiveUI;
using System.Reactive.Linq;
using System;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    
    public void NavigateToEditInstructions()
    {
        RecipeInstructionEditViewModel instructionViewModel = new();
        instructionViewModel.Save.Subscribe(instructions => 
        {
            if (instructions != null && instructions.Count >= 1)
            {
                NavigateToEditRecipe();
            }
        });

        ContentViewModel = instructionViewModel;
    }

    public void NavigateToEditRecipe()
    {
        ContentViewModel = new RecipeEditViewModel(); 
    }
    public MainWindowViewModel()
    {
        this._contentViewModel = new RecipeEditViewModel();
    }
}