using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Net;
using filtering;
using users;
[assembly: InternalsVisibleTo("project-application-test")]

namespace recipes;
public class RecipeController
{
    // as the user adds a filter, this will accumilate the filters (will NOT add one if already there.)
    public List<IFilterBy> Filters { get; } = new();
    public List<Recipe> AllRecipes { get; } = new();
    public List<Ingredient> Ingredients { get; } = new();

    private static RecipeController? _instance;

    private RecipeController() { }

    public static RecipeController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RecipeController();
            }
            return _instance;
        }
    }

    // gets list of recipes from db, adds recipe, sends back list to db
    public void CreateRecipe(Recipe recipe)
    {
        if (AllRecipes.Contains(recipe))
        {
            throw new ArgumentException("this recipe and name already exist in the database!");
        }

        if (!recipe.Owner.Equals(UserController.Instance.ActiveUser))
        {
            throw new ArgumentException("You cannot create a recipe that you are not the owner of");
        }

        AllRecipes.Add(recipe);

    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (!Ingredients.Contains(ingredient))
        {
            Ingredients.Add(ingredient);
        }
    }

    // get list of recipes, and remove particular recipe (VALIDATE THE USER TRYING TO DELETE)
    public void DeleteRecipe(Recipe recipe)
    {
        if (!AllRecipes.Contains(recipe))
        {
            throw new ArgumentException("This recipe does not exist in the database");
        }
        if (!recipe.Owner.Equals(UserController.Instance.ActiveUser))
        {
            throw new ArgumentException("Cannot delete the recipe you arent an owner of");
        }

        AllRecipes.Remove(recipe);
    }


    // filters all recipes using the filters in the list
    public List<Recipe> FilterBy()
    {
        List<Recipe> filtered = AllRecipes.ConvertAll(x => new Recipe(x));
        foreach (IFilterBy filter in Filters)
        {
            filtered = filter.FilterRecipes(filtered);
        }
        return filtered;
    }

    public void AddFilter(IFilterBy filter)
    {
        if (Filters.Contains(filter))
        {
            throw new ArgumentException("this filter already exists");
        }
        Filters.Add(filter);
    }

    public void RemoveFilter(IFilterBy filter)
    {
        if (!Filters.Contains(filter))
        {
            throw new ArgumentException("this filter hasnt been applied yet");
        }
        Filters.Remove(filter);
    }

}