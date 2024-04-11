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
    // gets list of recipes from db, adds recipe, sends back list to db
    public void CreateRecipe(Recipe  recipe, User owner)
    {
        throw new NotImplementedException();
    }
    // get list of recipes, and remove particular recipe (VALIDATE THE USER TRYING TO DELETE)
    public void DeleteRecipe(Recipe recipe){}
    public List<Recipe> FilterBy()
    {
        throw new NotImplementedException();
    }
    
}