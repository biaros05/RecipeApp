using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;


public class RecipeViewModel : ViewModelBase
{
    Measurement mg = new(Units.Mass, 0.001, 1000);
    Measurement g = new(Units.Mass, 1, 1);
    Measurement kg = new(Units.Mass, 1000, 0.001);
    Measurement mL = new(Units.Volume, 1, 1);
    Measurement L = new(Units.Volume, 1000, 0.001);
    Measurement gallon = new(Units.Volume, 3785.41, 0.000264172);
    private ObservableCollection<MeasuredIngredient> _ingredients;
    public ObservableCollection<MeasuredIngredient> Ingredients
    {
        get => _ingredients;
        set => this.RaiseAndSetIfChanged(ref _ingredients, value);
    }
    private bool _mlChecked = true;
    public bool MlChecked
    {
        get => _mlChecked;
        set
        {
            if (value == true)
            {
                Ingredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
            }
            this.RaiseAndSetIfChanged(ref _mlChecked, value);
        }
    }
    private bool _lChecked;
    public bool LChecked
    {
        get => _lChecked;
        set
        {
            if (value == true)
            {
                ObservableCollection<MeasuredIngredient> newIngredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
                foreach (MeasuredIngredient i in newIngredients)
                {
                    if (i.Ingredient.Unit == Units.Volume)
                    {
                        i.Quantity = mL.ConvertTo(L, i.Quantity);
                    }
                }
                Ingredients = newIngredients;
            }

            this.RaiseAndSetIfChanged(ref _lChecked, value);
        }
    }
    private bool _gallonChecked;
    public bool GallonChecked
    {
        get => _gallonChecked;
        set
        {
            if (value == true)
            {
                ObservableCollection<MeasuredIngredient> newIngredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
                foreach (MeasuredIngredient i in newIngredients)
                {
                    if (i.Ingredient.Unit == Units.Volume)
                    {
                        i.Quantity = mL.ConvertTo(gallon, i.Quantity);
                    }
                }
                Ingredients = newIngredients;
            }
            this.RaiseAndSetIfChanged(ref _gallonChecked, value);
        }
    }
    private bool _mgChecked;
    public bool MgChecked
    {
        get => _mgChecked;
        set
        {
            if (value == true)
            {
                ObservableCollection<MeasuredIngredient> newIngredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
                foreach (MeasuredIngredient i in newIngredients)
                {
                    if (i.Ingredient.Unit == Units.Mass)
                    {
                        i.Quantity = g.ConvertTo(mg, i.Quantity);
                    }
                }
                Ingredients = newIngredients;
            }
            this.RaiseAndSetIfChanged(ref _mgChecked, value);
        }
    }
    private bool _gChecked = true;
    public bool GChecked
    {
        get => _gChecked;
        set
        {
            if (value == true)
            {
                Ingredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
            }
            this.RaiseAndSetIfChanged(ref _gChecked, value);
        }
    }
    private bool _kgChecked;
    public bool KgChecked
    {
        get => _kgChecked;
        set
        {
            if (value == true)
            {
                ObservableCollection<MeasuredIngredient> newIngredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
                foreach (MeasuredIngredient i in newIngredients)
                {
                    if (i.Ingredient.Unit == Units.Mass)
                    {
                        i.Quantity = g.ConvertTo(kg, i.Quantity);
                    }
                }
                Ingredients = newIngredients;
            }

            this.RaiseAndSetIfChanged(ref _kgChecked, value);
        }
    }
    public Recipe CurrentRecipe { get; set; }
    public List<Instruction> Instructions { get; set; }
    private string _favoriteText;
    public string FavoriteText
    {
        get => _favoriteText;
        set => this.RaiseAndSetIfChanged(ref _favoriteText, value);
    }
    private string _rating;
    public string Rating
    {
        get => _rating;
        set => this.RaiseAndSetIfChanged(ref _rating, value);
    }
    private string _difficultyRating;
    public string DifficultyRating
    {
        get => _difficultyRating;
        set => this.RaiseAndSetIfChanged(ref _difficultyRating, value);
    }
    private int _givenRating;
    public int GivenRating
    {
        get => _givenRating;
        set => this.RaiseAndSetIfChanged(ref _givenRating, value);
    }
    private int _givenDifficultyRating;
    public int GivenDifficultyRating
    {
        get => _givenDifficultyRating;
        set => this.RaiseAndSetIfChanged(ref _givenDifficultyRating, value);
    }
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    public ReactiveCommand<Unit, Unit> RateCommand { get; }
    public ReactiveCommand<Unit, Unit> DifficultyCommand { get; }
    public ReactiveCommand<Unit, Unit> FavoriteCommand { get; }
    public ReactiveCommand<Unit, Unit> ConvertCommand { get; }
    public RecipeViewModel(Recipe recipe)
    {
        CurrentRecipe = recipe;
        Instructions = recipe.Instructions;
        //NOTE - Sorts the recipe by the indexers
        Instructions.Sort(delegate (Instruction a, Instruction b)
        {
            if (a.Index == b.Index) return 0;
            else if (a.Index < b.Index) return -1;
            else return 1;
        });
        _ingredients = new(CurrentRecipe.Ingredients.ConvertAll(s => s.Clone()).ToList());
        if (recipe.Rating == 0 || recipe.Rating == null)
        {
            _rating = "No Ratings";
        }
        else
        {
            _rating = recipe.Rating.ToString();
        }
        if (recipe.DifficultyRating == 0 || recipe.DifficultyRating == null)
        {
            _difficultyRating = "No Ratings";
        }
        else
        {
            _difficultyRating = recipe.DifficultyRating.ToString();
        }

        FavoriteCommand = ReactiveCommand.Create(() =>
        {
            if (UserController.Instance.ActiveUser.UserFavoriteRecipies.Contains(CurrentRecipe))
            {
                UserController.Instance.ActiveUser.RemoveFromFavourites(CurrentRecipe);
            }
            else
            {
                UserController.Instance.ActiveUser.AddToFavourites(CurrentRecipe);
            }
            FavoriteText = "Add to Favorites";
            foreach (Recipe r in UserController.Instance.ActiveUser.UserFavoriteRecipies)
            {
                if (r.Name == CurrentRecipe.Name)
                {
                    FavoriteText = "Remove from Favorite";
                }
            }

        });

        RateCommand = ReactiveCommand.Create(() =>
        {
            if (GivenRating < 1 || GivenRating > 5)
            {
                ErrorMessage = "Cannot give rating of 0 or greater than 5";
            }
            else
            {
                try
                {
                    CurrentRecipe.RateRecipe(GivenRating, UserController.Instance.ActiveUser);
                    ErrorMessage = "Rating added";
                }
                catch (ArgumentOutOfRangeException)
                {
                    ErrorMessage = "Rating has to be between 1 and 5";
                }
                catch (Exception)
                {
                    for (int i = 0; i < CurrentRecipe._ratings.Count; i++)
                    {
                        if (CurrentRecipe._ratings[i].Owner == UserController.Instance.ActiveUser)
                        {
                            CurrentRecipe._ratings[i].StarRating = GivenRating;
                            ErrorMessage = "Rating updated";
                        }
                    }
                }
                if (recipe.Rating == 0 || recipe.Rating == null)
                {
                    Rating = "No Ratings";
                }
                else
                {
                    Rating = recipe.Rating.ToString();
                }
            }
        });

        DifficultyCommand = ReactiveCommand.Create(() =>
        {
            if (GivenDifficultyRating < 1 || GivenDifficultyRating > 10)
            {
                ErrorMessage = "Cannot give rating of 0 or greater than 10";
            }
            else
            {
                try
                {
                    CurrentRecipe.RateDifficulty(GivenDifficultyRating, UserController.Instance.ActiveUser);
                    ErrorMessage = "Difficulty added";
                }
                catch (ArgumentOutOfRangeException)
                {
                    ErrorMessage = "Difficulty has to be between 1 and 10";
                }
                catch (Exception)
                {
                    for (int i = 0; i < CurrentRecipe._difficulties.Count; i++)
                    {
                        if (CurrentRecipe._difficulties[i].Owner == UserController.Instance.ActiveUser)
                        {
                            CurrentRecipe._difficulties[i].ScaleRating = GivenDifficultyRating;
                            ErrorMessage = "Difficulty updated";
                        }
                    }
                }
                if (recipe.DifficultyRating == 0 || recipe.DifficultyRating == null)
                {
                    DifficultyRating = "No Ratings";
                }
                else
                {
                    DifficultyRating = recipe.DifficultyRating.ToString();
                }
            }
        });
        FavoriteText = "Add to Favorites";
        foreach (Recipe r in UserController.Instance.ActiveUser.UserFavoriteRecipies)
        {
            if (r.Name == CurrentRecipe.Name)
            {
                FavoriteText = "Remove from Favorite";
            }
        }

    }
}