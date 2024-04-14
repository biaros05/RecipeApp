using System.Collections.Generic;
namespace recipes;

using System.Globalization;
using System.Net;
using filtering;
using users;
public class RecipeController
{
    // as the user adds a filter, this will accumilate the filters (will NOT add one if already there.)
    public static List<IFilterBy> Filters {get;} = new();

    public static List<Recipe> AllRecipes{get;} = new();
    public static List<Ingredient> Ingredients {get;} = new();

    // gets list of recipes from db, adds recipe, sends back list to db
    public static void CreateRecipe(Recipe recipe, User owner)
    {
        if (AllRecipes.Contains(recipe))
        {
            throw new ArgumentException("this recipe already exists in the database!");
        }

        // check for the name?

        AllRecipes.Add(recipe);
    }

    public static void AddIngredient(Ingredient ingredient)
    {
        if (!Ingredients.Contains(ingredient))
        {
            Ingredients.Add(ingredient);
        }
    }

    // get list of recipes, and remove particular recipe (VALIDATE THE USER TRYING TO DELETE)
    public static void DeleteRecipe(Recipe recipe)
    {
        if (!AllRecipes.Contains(recipe))
        {
            throw new ArgumentException("This recipe does not exist in the database");
        }
        AllRecipes.Remove(recipe);
    }
    

    // must clone the recipes!!!!
    public static List<Recipe> FilterBy()
    {
        List<Recipe> filtered = AllRecipes.ConvertAll(x => new Recipe(x));
        foreach (IFilterBy filter in Filters)
        {
            filtered = filter.FilterRecipes(filtered);
        }
        return filtered;
    }
    
}