using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Net;
using filtering;
using users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
[assembly: InternalsVisibleTo("project-application-test")]

namespace recipes;
public class RecipeController
{
    public List<IFilterBy> Filters { get; set; } = new();
    private List<Recipe> allRecipes = new();
    //NOTE - might not be the good way to do this
    public List<Recipe> AllRecipes
    {
        get
        {
            var context = RecipesContext.Instance;
            List<Recipe> retrieveRecipes = context.RecipeManager_Recipes
                .Include(recipe => recipe.Tags)
                .Include(recipe => recipe._ratings)
                .Include(recipe => recipe._difficulties)
                .Include(recipe => recipe.Owner)
                .Include(recipe => recipe.Ingredients)
                    .ThenInclude(measuredIngredient => measuredIngredient.Ingredient)
                .Include(recipe => recipe.Instructions)
                .Include(recipe => recipe.UserFavourite)
                .ToList();
            allRecipes = retrieveRecipes;

            return allRecipes;
        }
    }
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
        set
        {
            _instance = value;
        }
    }

    /// <summary>
    /// this method will add the new recipe to the database. 
    /// it will check that it doesnt yet exist and will also make sure the proper user is logged in before creating
    /// </summary>
    /// <param name="recipe">The recipe to create</param>
    public static void CreateRecipe(Recipe recipe)
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

    /// <summary>
    ///  this will add am ingredient to the database. 
    /// if it already exists, it will refrain from adding
    /// </summary>
    /// <param name="ingredient">The ingredient to add</param>
    public static void AddIngredient(Ingredient ingredient)
    {
        var context = RecipesContext.Instance;
        List<Ingredient> retrieveIngredients = context.RecipeManager_Ingredients.ToList<Ingredient>();
        if (!retrieveIngredients.Contains(ingredient))
        {
            context.RecipeManager_Ingredients.Add(ingredient);
            context.SaveChanges();
        }
    }

    /// <summary>
    /// this method will delete a recipe from the database. if the recipe does not exist
    /// or if the logged in user is not the recipe's owner, it will throw an exception
    /// </summary>
    /// <param name="recipe">The recipe to delete</param>

    // filters all recipes using the filters in the list **********
    public static void DeleteRecipe(Recipe recipe)
    {
        var context = RecipesContext.Instance;
        IQueryable<Recipe> recipeQuery = context.RecipeManager_Recipes;
        IQueryable<Recipe> retrieveRecipes = RecipesContext.Instance.RecipeManager_Recipes
            .Include(recipe => recipe.Tags)
            .Include(recipe => recipe._ratings)
            .Include(recipe => recipe._difficulties)
            .Include(recipe => recipe.Owner)
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.UserFavourite);
        Recipe? toRemove = retrieveRecipes
            .Include(recipe => recipe.Tags)
            .Include(recipe => recipe._ratings)
            .Include(recipe => recipe._difficulties)
            .Include(recipe => recipe.Owner)
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.UserFavourite)
            .FirstOrDefault(r => r.Name.Equals(recipe.Name) && r.Owner.Equals(recipe.Owner));
        if (toRemove == null)
        {
            throw new ArgumentException("This recipe does not exist in the database");
        }
        if (!toRemove.Owner.Equals(UserController.Instance.ActiveUser))
        {
            throw new ArgumentException("Cannot delete the recipe you arent an owner of");
        }

        RecipesContext.Instance.RecipeManager_Recipes.Remove(toRemove);
        RecipesContext.Instance.SaveChanges();
    }


    /// <summary>
    /// this method will apply all the filters in the current Filters list 
    /// and filter the recipes in the database, returning a list of recipes
    /// </summary>
    /// <returns>List of filtered recipes</returns>
    public List<Recipe> FilterBy()
    {
        var context = RecipesContext.Instance;
        var recipeQuery = context.RecipeManager_Recipes
            .Include(recipe => recipe.Tags)
            .Include(recipe => recipe._ratings)
            .Include(recipe => recipe._difficulties)
            .Include(recipe => recipe.Owner)
            .Include(recipe => recipe.Ingredients)
                .ThenInclude(measuredIngredient => measuredIngredient.Ingredient)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.UserFavourite)
            .AsQueryable();
        foreach (IFilterBy filter in Filters)
        {
            recipeQuery = filter.FilterRecipes(recipeQuery);
        }
        return recipeQuery.ToList();
    }

    /// <summary>
    /// this method will add a filter to the list if it does not already exits
    /// </summary>
    /// <param name="filter">The filter to add</param>
    public void AddFilter(IFilterBy filter)
    {
        if (Filters.Contains(filter))
        {
            throw new ArgumentException("this filter already exists");
        }
        Filters.Add(filter);
    }

    /// <summary>
    /// this method will remove a filter from the list if it exists
    /// </summary>
    /// <param name="filter">The filter to remove</param>
    public void RemoveFilter(IFilterBy filter)
    {
        if (!Filters.Contains(filter))
        {
            throw new ArgumentException("this filter hasnt been applied yet");
        }
        Filters.Remove(filter);
    }

    /// <summary>
    /// this method effectively clears all the filters from the Filters list
    /// </summary>
    public void RemoveAllFilters()
    {
        Filters.Clear();
    }

}