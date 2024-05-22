using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using filtering;
using ReactiveUI;
using recipes;
using users;
using Avalonia.Controls;
using System;

namespace App.ViewModels;

public class RecipeListViewModel : ViewModelBase
{
    private ObservableCollection<Recipe> recipeList;
    public ObservableCollection<Recipe> RecipeList
    {
        get => recipeList;
        set => this.RaiseAndSetIfChanged(ref recipeList, value);
    }
    public ObservableCollection<IFilterBy> FilterList
    { get; set; } = new();
    private string? _keyword;
    public string? Keyword
    {
        get => _keyword;
        set
        {
            try
            {

                this.RaiseAndSetIfChanged(ref _keyword, value);
            }
            catch (Exception)
            {
                FilterErrorMessage = "Keyword Error";
            }

        }
    }
    private string? _ingredients;
    public string? Ingredients
    {
        get => _ingredients;
        set => this.RaiseAndSetIfChanged(ref _ingredients, value);
    }
    private string? _tags;
    public string? Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }
    private string? _owner;
    public string? Owner
    {
        get => _owner;
        set
        {
            this.RaiseAndSetIfChanged(ref _owner, value);
            if (value != null)
            {
                try
                {
                    if (value.Length < 5 || value.Length > 50)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    FilterErrorMessage = null;
                }
                catch (Exception)
                {
                    FilterErrorMessage = "owner must be between 5, 50";
                }
            }
        }
    }
    private int _rating;
    public int Rating
    {
        get => _rating;
        set
        {
            try
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.RaiseAndSetIfChanged(ref _rating, value);
                FilterErrorMessage = null;
            }
            catch (Exception)
            {
                FilterErrorMessage = "Rating Error";
            }
        }
    }
    private int _minServing;
    public int MinServing
    {
        get => _minServing;
        set
        {
            this.RaiseAndSetIfChanged(ref _minServing, value);
            try
            {
                if (value < 0 || value > _maxServing)
                {
                    throw new ArgumentOutOfRangeException();
                }
                FilterErrorMessage = null;
            }
            catch (Exception)
            {
                FilterErrorMessage = "min has to be < than max";
            }
        }
    }
    private int _maxServing;
    public int MaxServing
    {
        get => _maxServing;
        set
        {
            this.RaiseAndSetIfChanged(ref _maxServing, value);
            try
            {
                if (value < 0 || value < _minServing)
                {
                    throw new ArgumentOutOfRangeException();
                }
                FilterErrorMessage = null;
            }
            catch (Exception)
            {
                FilterErrorMessage = "min has to be < than max";
            }
        }
    }
    private int _minTime;
    public int MinTime
    {
        get => _minTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _minTime, value);
            try
            {
                if (value < 0 || value > _maxTime || value > 420)
                {
                    throw new ArgumentOutOfRangeException();
                }
                FilterErrorMessage = null;
            }
            catch (Exception)
            {
                FilterErrorMessage = "time has to be between 0 and 420 and min < max";
            }
        }
    }
    private int _maxTime;
    public int MaxTime
    {
        get => _maxTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _maxTime, value);
            try
            {
                if (value < 0 || value < _minTime || value > 420)
                {
                    throw new ArgumentOutOfRangeException();
                }
                FilterErrorMessage = null;
            }
            catch (Exception)
            {
                FilterErrorMessage = "time has to be between 0 and 420 and min < max";
            }
        }
    }
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    private string? _filterErrorMessage;
    public string? FilterErrorMessage
    {
        get => _filterErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _filterErrorMessage, value);
    }
    private string? _favoriteText;
    public string? FavoriteText
    {
        get => _favoriteText;
        set => this.RaiseAndSetIfChanged(ref _favoriteText, value);
    }
    private Recipe? _selectedRecipe;
    public Recipe? SelectedRecipe
    {
        get => _selectedRecipe;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedRecipe, value);
            FavoriteText = "Add to Favorites";
            if (_selectedRecipe != null)
            {
                foreach (Recipe r in UserController.Instance.ActiveUser.UserFavoriteRecipies)
                {
                    if (r.Name == SelectedRecipe.Name)
                    {
                        FavoriteText = "Remove from Favorite";
                    }
                }
            }
        }
    }
    public ReactiveCommand<Unit, ObservableCollection<IFilterBy>> AddFilterCommand { get; }
    public ReactiveCommand<Unit, ObservableCollection<Recipe>> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearFilterCommand { get; }
    public ReactiveCommand<Unit, Recipe?> ViewRecipeCommand { get; }
    public ReactiveCommand<Unit, Unit> FavoriteCommand { get; }
    public RecipeListViewModel()
    {
        AddFilterCommand = ReactiveCommand.Create(() =>
        {
            if (_keyword != null && _keyword != "")
            {
                RecipeController.Instance.AddFilter(new FilterByKeyword(_keyword));
            }
            if (_ingredients != null && _ingredients != "")
            {
                string[] splitString = _ingredients.Split(','); // splits the string by ','
                List<Ingredient> ingList = new();
                foreach (string s in splitString)
                {
                    //NOTE - trims the string whitespace from front and back
                    // creates a new empty ingredient and add name for filtering
                    Ingredient ingredient = new();
                    ingredient.Name = s.Trim();
                    ingList.Add(ingredient);
                }
                RecipeController.Instance.AddFilter(new FilterByIngredients(ingList));
            }
            if (_tags != null && _tags != "")
            {
                string[] splitString = _tags.Split(',');
                List<Tag> tagList = new();
                foreach (string s in splitString)
                {
                    Tag tag = new();
                    tag.TagName = s.Trim();
                    tagList.Add(tag);
                }
                RecipeController.Instance.AddFilter(new FilterByTags(tagList));
            }
            if (_owner != null && _owner != "")
            {
                User owner = new();
                owner.Username = _owner;
                // var abc = new FilterByUsername(RecipesContext.Instance.RecipeManager_Users);
                // var owner = abc.FilterUsers(_owner);
                RecipeController.Instance.AddFilter(new FilterByOwner(owner));
            }
            if (_rating != 0)
            {
                RecipeController.Instance.AddFilter(new FilterByRating(_rating));
            }
            if (_minServing != 0 && _maxServing > _minServing)
            {
                RecipeController.Instance.AddFilter(new FilterByServings(_minServing, _maxServing));
            }
            if (_minTime != 0 && _maxTime > _minTime && _maxTime < 420)
            {
                RecipeController.Instance.AddFilter(new FilterByTime(_minTime, _maxTime));
            }
            FilterList.Clear();
            RecipeController.Instance.Filters.ForEach(filter => FilterList.Add(filter));
            // FilterList = new ObservableCollection<IFilterBy>(RecipeController.Instance.Filters);
            return FilterList;
        });

        ClearFilterCommand = ReactiveCommand.Create(() =>
        {
            RecipeController.Instance.RemoveAllFilters();
            // FilterList = new ObservableCollection<IFilterBy>(RecipeController.Instance.Filters);
            FilterList.Clear();
            RecipeList = new ObservableCollection<Recipe>(RecipeController.Instance.AllRecipes);
        });

        SearchCommand = ReactiveCommand.Create(() =>
        {
            RecipeList = new ObservableCollection<Recipe>(RecipeController.Instance.FilterBy());
            return RecipeList;
        });
        ViewRecipeCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedRecipe == null)
            {
                ErrorMessage = "Select a recipe";
                return null;
            }
            return SelectedRecipe;
        });
        FavoriteCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedRecipe == null)
            {
                ErrorMessage = "Select a recipe";
            }
            else
            {
                if (UserController.Instance.ActiveUser.UserFavoriteRecipies.Contains(SelectedRecipe))
                {
                    UserController.Instance.ActiveUser.RemoveFromFavourites(SelectedRecipe);
                    ErrorMessage = "Removed from favorites";
                    FavoriteText = "Add to Favorites";
                }
                else
                {
                    UserController.Instance.ActiveUser.AddToFavourites(SelectedRecipe);
                    ErrorMessage = "Added To Favorites";
                    FavoriteText = "Remove from Favorites";
                }
            }
        });
        recipeList = new ObservableCollection<Recipe>();
        FavoriteText = "Add to Favorites";
        RecipeController.Instance.RemoveAllFilters();
    }
}