using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.LogicalTree;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;

public class RecipeCreateViewModel : ViewModelBase
{
    private Recipe Recipe {get; set;} = new();
    public string? Name
    {
        get => Recipe.Name;
        set => Recipe.Name = value;
    }

    public string? Description
    {
        get => Recipe.Description;
        set => Recipe.Description = value;
    }
    public int PrepTime
    {
        get => Recipe.PrepTimeMins;
        set => Recipe.PrepTimeMins = Convert.ToInt32(value);
    }
    public int CookTime 
    {
        get => Recipe.CookTimeMins;
        set => Recipe.CookTimeMins = Convert.ToInt32(value);
    }
    public int NumServings 
    {
        get => Recipe.NumberOfServings;
        set => Recipe.NumberOfServings = Convert.ToInt32(value);
    }
    
    public int Budget 
    {
        get => Recipe.Budget.Length;
        set
        {
            if (value < 1 || value > 3)
            {
                throw new ArgumentException("Budget rating must be 1, 2, or 3");
            }
            this.Recipe.Budget = new string('$', Convert.ToInt32(value));
        }
    }
    
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    } 

    public ObservableCollection<Instruction> Instructions = new();

    public ObservableCollection<MeasuredIngredient> Ingredients = new();
    public ObservableCollection<Tag> Tags = new();

    // reactive command for the Save button
    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Recipe> TagButton { get; }

    public ReactiveCommand<Unit, Recipe> IngredientButton { get; }

    public ReactiveCommand<Unit, Recipe> InstructionButton { get; }

    public RecipeCreateViewModel(Recipe? recipe=null)
    {
        this.Recipe = new(){Budget = ""};
        this.Recipe.Owner = UserController.Instance.ActiveUser!;
        if (recipe != null)
        {
            this.Recipe = recipe;
        }
        this.Instructions = new(this.Recipe.Instructions);
        this.Ingredients = new(this.Recipe.Ingredients);
        this.Tags = new(this.Recipe.Tags);
        
        // validates that name textbox is filled
        IObservable<bool> areFieldsFilled = this.WhenAnyValue(
        recipeViewModel => recipeViewModel.Name,
        (name) =>
            !(string.IsNullOrEmpty(name))
        );
        IObservable<bool> atLeastOneInstruction = this.WhenAnyValue(vm => vm.Instructions.Count).Select(count => count > 0);
        IObservable<bool> atLeastOneIngredient = this.WhenAnyValue(vm => vm.Ingredients.Count).Select(count => count > 0);

        IObservable<bool> allConditionsMet = Observable.CombineLatest(
            areFieldsFilled,
            atLeastOneInstruction,
            atLeastOneIngredient,
            (hasFields, hasInstruction, hasIngredient) => hasFields && hasInstruction && hasIngredient
        );

        Save = ReactiveCommand.Create(() =>
            {
                try{
                    RecipeController.CreateRecipe(this.Recipe);
                }
                catch(Exception e)
                {
                    ErrorMessage = e.Message;
                }
            }, allConditionsMet
        );

        InstructionButton = ReactiveCommand.Create(() =>
        {
            return this.Recipe;
        });

        IngredientButton = ReactiveCommand.Create(() =>
        {
            return this.Recipe;
        });

        TagButton = ReactiveCommand.Create(() => 
        {
            return this.Recipe;
        });
    }
}