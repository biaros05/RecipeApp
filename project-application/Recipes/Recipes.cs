using System.Collections.Generic;
namespace recipes;

using sorting;
using users;
public class Recipes{
    // gets list of recipes from db, adds recipe, sends back list to db
    public void CreateRecipe(Recipe  recipe, User owner)
    {
        throw new NotImplementedException();
    }
    // get list of recipes, and remove particular recipe
    public void DeleteRecipe(Recipe recipe){}
    public List<IFilterBy> Filters {get;} = new();
    public List<Recipes> FilterBy()
    {
        throw new NotImplementedException();
    }
    
}