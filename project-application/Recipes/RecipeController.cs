using System.Collections.Generic;
namespace recipes;

using System.Net;
using filtering;
using users;
public class RecipeController
{
    // as the user adds a filter, this will accumilate the filters (will NOT add one if already there.)
    public List<IFilterBy> Filters {get;} = new();

    public static List<Recipe> AllRecipes{get;} = new();
    public static List<Ingredient> Ingredients {get;} = new();

    // gets list of recipes from db, adds recipe, sends back list to db
    public void CreateRecipe(Recipe recipe, User owner)
    {
        if (AllRecipes.Contains(recipe))
        {
            throw new ArgumentException("this recipe already exists in the database!");
        }

        // check for the name?

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
        AllRecipes.Remove(recipe);
    }
    
    public List<Recipe> FilterBy()
    {
        foreach (IFilterBy filter in this.Filters)
        {
            filter.FilterRecipes(AllRecipes);
        }
        return AllRecipes;
    }
    
}