using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData.Binding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;

public class RecipeIngredientEditViewModel : ViewModelBase
{
    private Recipe Recipe {get; set;} = new();

    // User ingredients
    private ObservableCollection<MeasuredIngredient> _measuredIngredients = new();

    public ObservableCollection<MeasuredIngredient> MeasuredIngredients
    {
        get => _measuredIngredients;
        set => this.RaiseAndSetIfChanged(ref _measuredIngredients, value);
    }

    // Functionality to add ingredient to recipe

    private ObservableCollection<Ingredient> _dbIngredients;
    public ObservableCollection<Ingredient> DBIngredients
    {
        get => _dbIngredients;
        set => this.RaiseAndSetIfChanged(ref _dbIngredients, value);
    }
    
    private Ingredient? _selectedIngredient;
    public Ingredient? SelectedIngredient
    {
        get => _selectedIngredient;
        set => this.RaiseAndSetIfChanged(ref _selectedIngredient, value);
    }

    private double? _quantity;
    public double? Quantity
    {
        get => _quantity;
        set => this.RaiseAndSetIfChanged(ref _quantity, Convert.ToInt32(value));
    }

    public ReactiveCommand<Unit, Unit> Add { get; }

    // functionality to add new ingredient to DB
    private string? _newIngredientName;
    public string? NewIngredientName
    {
        get => _newIngredientName;
        set => this.RaiseAndSetIfChanged(ref _newIngredientName, value);
    }

    private int? _newUnit;
    public int? NewUnit
    {
        get => _newUnit;
        set
        {
            SelectedUnit = (Units) Convert.ToInt32(value);
            this.RaiseAndSetIfChanged(ref _newUnit, Convert.ToInt32(value));
        }
    }



    private Units? _selectedUnit;
    public Units? SelectedUnit
    {
        get => _selectedUnit;
        set => _selectedUnit = value;
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    } 

    public ReactiveCommand<Unit, Unit> AddDB { get; }

    // Save the added ingredients to recipe or cancel
    public ReactiveCommand<Unit, Recipe> Save { get; }
    public ReactiveCommand<Unit, Recipe> Cancel { get; }

    public RecipeIngredientEditViewModel(Recipe? recipe = null)
    {
        this.Recipe = new();
        if (recipe != null)
            this.Recipe = recipe;

        this.MeasuredIngredients = new(this.Recipe.Ingredients);

        this.DBIngredients = new(RecipesContext.Instance.RecipeManager_Ingredients);

        // Functionality to add ingredient to recipe

        IObservable<bool> notEmpty = this.WhenAnyValue(
            recipeViewModel => recipeViewModel.SelectedIngredient,
            recipeViewModel => recipeViewModel.Quantity,
            (ingr, quantity) => ingr != null && quantity != null && quantity > 0
        );
        
        Add = ReactiveCommand.Create(() => 
        {
            try{
                MeasuredIngredient newIngr = new(this.SelectedIngredient!, (double)this.Quantity!);
                if (!this.MeasuredIngredients.Contains(newIngr))
                {
                    MeasuredIngredients.Add(newIngr);
                    this.SelectedIngredient = null;
                    this.Quantity = null;
                }
            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
            }

        }, notEmpty);

        // functionality to add new ingredient to list of available
        IObservable<bool> newIngredientFieldsValid = this.WhenAnyValue(
            recipeViewModel => recipeViewModel.NewIngredientName,
            recipeViewModel => recipeViewModel.NewUnit,
            (ingr, unit) => !(string.IsNullOrEmpty(ingr)) && unit != null
        );

        AddDB = ReactiveCommand.Create(() => 
        {
            try
            {
                Ingredient newIngr = new(this.NewIngredientName!, (Units)this.SelectedUnit!);
                if (!this.DBIngredients.Contains(newIngr))
                {
                    this.DBIngredients.Add(newIngr);
                    this.NewIngredientName = "";
                }
                else
                {
                    this.ErrorMessage = "Ingredient already exists";
                }
            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
            }
        }, newIngredientFieldsValid);

        // save and cancel functionalities
        IObservable<bool> ingredientsAvailable = this.WhenAnyValue(vm => vm.MeasuredIngredients.Count).Select(count => count > 0);

        Save = ReactiveCommand.Create(() => 
        {
            this.Recipe.Ingredients = this.MeasuredIngredients.ToList();
            return Recipe;
        }, ingredientsAvailable);

        Cancel = ReactiveCommand.Create(() =>
        {
            return Recipe;
        });

    }
}