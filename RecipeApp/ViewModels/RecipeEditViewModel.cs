using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;

public class RecipeEditViewModel : ViewModelBase
{
    private Recipe? Recipe {get; set;}
    private string? _name;
    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    private int _prepTime;
    public int PrepTime
    {
        get => _prepTime;
        set => this.RaiseAndSetIfChanged(ref _prepTime, Convert.ToInt32(value));
    }
    private int _cookTime;
    public int CookTime 
    {
        get => _cookTime;
        set => this.RaiseAndSetIfChanged(ref _cookTime, Convert.ToInt32(value));
    }
    private int _numServings;
    public int NumServings 
    {
        get => _numServings;
        set => this.RaiseAndSetIfChanged(ref _numServings, Convert.ToInt32(value));
    }
    
    private int _budget;
    public int Budget 
    {
        get => _budget;
        set => this.RaiseAndSetIfChanged(ref _budget, Convert.ToInt32(value));
    }
    
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    } 

    public ObservableCollection<Instruction> Instructions;

    public ObservableCollection<MeasuredIngredient> Ingredients;
    public ObservableCollection<Tag> Tags;

    // reactive command for the Save button
    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Recipe?> TagButton { get; }

    public ReactiveCommand<Unit, Recipe?> IngredientButton { get; }

    public ReactiveCommand<Unit, Recipe?> InstructionButton { get; }

    public RecipeEditViewModel()
    {
        // validates that name textbox is filled (FIXME: ADD INGREDIENTS AND INSTRUCTIONS)
        IObservable<bool> areFieldsFilled = this.WhenAnyValue(
        recipeViewModel => recipeViewModel.Name,
        (name) =>
            !(string.IsNullOrEmpty(name))
        );
        Save = ReactiveCommand.Create(() =>
            {
                Recipe recipe = new Recipe(Name, UserController.Instance.ActiveUser, Description, PrepTime, CookTime, NumServings, Instructions.ToList(), Ingredients.ToList(), Tags.ToList(), Budget);
                try{
                    RecipeController.CreateRecipe(recipe);
                }
                catch(Exception e)
                {
                    ErrorMessage = e.Message;
                }
            }, areFieldsFilled
        );

        InstructionButton = ReactiveCommand.Create(() =>
            {
                return Recipe;
            }
        );
    }
}