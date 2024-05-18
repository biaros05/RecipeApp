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

public class RecipeEditViewModel : ViewModelBase
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

    public RecipeEditViewModel(Recipe? recipe=null)
    {
        UserController.Instance.AuthenticateUser("Bianca", "Rossetti");
        this.Recipe = new(){Budget = ""};
        this.Recipe.Owner = UserController.Instance.ActiveUser!;
        if (recipe != null)
        {
            this.Recipe = recipe;
        }

        // validates that name textbox is filled (FIXME: ADD INGREDIENTS AND INSTRUCTIONS)
        IObservable<bool> areFieldsFilled = this.WhenAnyValue(
        recipeViewModel => recipeViewModel.Name,
        (name) =>
            !(string.IsNullOrEmpty(name))
        );

        Save = ReactiveCommand.Create(() =>
            {
                try{
                    RecipeController.CreateRecipe(this.Recipe);
                    // navigates elsewhere
                }
                catch(Exception e)
                {
                    ErrorMessage = e.Message;
                }
            }, areFieldsFilled
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