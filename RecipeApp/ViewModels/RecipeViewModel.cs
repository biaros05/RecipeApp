using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using recipes;

namespace App.ViewModels;

public class RecipeViewModel : ViewModelBase
{
    public Recipe CurrentRecipe { get; set; }
    public List<Instruction> Instructions { get; set; }
    private List<MeasuredIngredient> _ingredients;
    public List<MeasuredIngredient> Ingredients
    {
        get => _ingredients;
        set => this.RaiseAndSetIfChanged(ref _ingredients, value);
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
    private string _givenRating;
    public string GivenRating
    {
        get => _givenRating;
        set => this.RaiseAndSetIfChanged(ref _givenRating, value);
    }
    private string _givenDifficultyRating;
    public string GivenDifficultyRating
    {
        get => _givenDifficultyRating;
        set => this.RaiseAndSetIfChanged(ref _givenDifficultyRating, value);
    }
    public ReactiveCommand<Unit, int> RateCommand { get; }
    public ReactiveCommand<Unit, int> DifficultyCommand { get; }
    public ReactiveCommand<Unit, Unit> FavoriteCommand { get; }
    public ReactiveCommand<Unit, Unit> ConvertCommand { get; }
    public RecipeViewModel(Recipe recipe)
    {
        CurrentRecipe = recipe;
        Instructions = recipe.Instructions;
        //NOTE - Sorts the recipe by the indexers
        Instructions.Sort(delegate(Instruction a, Instruction b)
        {
            if (a.Index == b.Index) return 0;
            else if (a.Index < b.Index) return -1;
            else return 1;
        });
        _ingredients = recipe.Ingredients;
        if (recipe.Rating == 0 || recipe.Rating == null)
        {
            _rating = "No Ratings";
        }
        else{
            _rating = recipe.Rating.ToString();
        }
        if (recipe.DifficultyRating == 0 || recipe.DifficultyRating == null)
        {
            _difficultyRating = "No Ratings";
        }
        else{
            _difficultyRating = recipe.DifficultyRating.ToString();
        }
    }
}