using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Net;
using filtering;
using users;
[assembly:  InternalsVisibleTo("project-application-test")]

namespace recipes;
public class RecipeController
{
    // as the user adds a filter, this will accumilate the filters (will NOT add one if already there.)
    public static List<IFilterBy> Filters {get; internal set;} = new();
    public static List<Recipe> AllRecipes {get; internal set;} = new();
    public static List<Ingredient> Ingredients {get; internal set;} = new();

    // gets list of recipes from db, adds recipe, sends back list to db
    public static void CreateRecipe(Recipe recipe)
    {
        if (AllRecipes.Contains(recipe))
        {
            throw new ArgumentException("this recipe and name already exist in the database!");
        }

        if (!recipe.Owner.Equals(UserController.ActiveUser))
        {
            throw new ArgumentException("You cannot create a recipe that you are not the owner of");
        }

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
        if (!recipe.Owner.Equals(UserController.ActiveUser))
        {
            throw new ArgumentException("Cannot delete the recipe you arent an owner of");
        }

        AllRecipes.Remove(recipe);
    }
    

    // filters all recipes using the filters in the list
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