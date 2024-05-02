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
    public List<IFilterBy> Filters { get; set;} = new();
    public List<Recipe> AllRecipes { get; } = new();
    public List<Ingredient> Ingredients { get; } = new();

    // singleton RecipeController in order to keep the instance up to date across entire program
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

    // will add the recipe to the list of all recipes
    public void CreateRecipe(Recipe recipe)
    {
        var context = RecipesContext.Instance;
        List<Recipe> retrieveRecipes = context.RecipeManager_Recipes.ToList<Recipe>();
        if (retrieveRecipes.Contains(recipe))
        {
            throw new ArgumentException("this recipe and name already exist in the database!");
        }

        if (!recipe.Owner.Equals(UserController.Instance.ActiveUser))
        {
            throw new ArgumentException("You cannot create a recipe that you are not the owner of");
        }
        context.RecipeManager_Recipes.Add(recipe);
        context.SaveChanges();

    }

    // adds an ingredient to the list of ingredients available for selection
    public void AddIngredient(Ingredient ingredient)
    {
        var context = RecipesContext.Instance;
        List<Ingredient> retrieveIngredients = RecipesContext.Instance.RecipeManager_Ingredients.ToList<Ingredient>();
        if (!retrieveIngredients.Contains(ingredient))
        {
            context.RecipeManager_Ingredients.Add(ingredient);
            context.SaveChanges();
        }
    }

    // get list of recipes, and remove particular recipe. only allows owner to remove recipe
    public static void DeleteRecipe(Recipe recipe)
    {
        var context = RecipesContext.Instance;
        List<Recipe> retrieveRecipes = context.RecipeManager_Recipes.ToList<Recipe>();
        if (!retrieveRecipes.Contains(recipe))
        {
            throw new ArgumentException("This recipe does not exist in the database");
        }
        if (!recipe.Owner.Equals(UserController.Instance.ActiveUser))
        {
            throw new ArgumentException("Cannot delete the recipe you arent an owner of");
        }

        context.Remove(recipe);
        context.SaveChanges();
    }


    // filters all recipes using the filters in the list **********
    public List<Recipe> FilterBy()
    {
        var context = RecipesContext.Instance;
        var recipeQuery = context.RecipeManager_Recipes.AsQueryable();
        foreach (IFilterBy filter in Filters)
        {
            recipeQuery = filter.FilterRecipes(recipeQuery);
        }
        return recipeQuery.ToList<Recipe>();
    }

    // as the user adds a filter, this will accumilate the filters (will NOT add one if already there.)

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

    public void RemoveAllFilters()
    {
        Filters = new();
    }

}