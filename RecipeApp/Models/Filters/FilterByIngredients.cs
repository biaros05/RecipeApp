namespace filtering;

using System;
using System.Collections.Generic;
using System.Linq;
using recipes;
// this class will filter by ingredients
public class FilterByIngredients : IFilterBy
{
    public FilterByIngredients(List<Ingredient> ingredients)
    {
        if (ingredients.Count() == 0 || ingredients == null)
        {
            throw new InvalidOperationException("ingredient list is null or empty");
        }
        Ingredients = ingredients;
        foreach (Ingredient i in ingredients)
        {
            Value += i.Name + ", ";
        }
        //NOTE - Removes last ', ' added
        Value = Value[..Value.LastIndexOf(", ")];
    }
    private List<Ingredient> Ingredients;
    private string value;

    public string Value { get => value; set => this.value = value; }

    public string Name => "Ingredients";

    /// <summary>
    /// Taken a list of Ingredients, compares and returns true if one ingredient matches with one ingredient in given list
    /// </summary>
    /// <param name="recipeIngredients">Ingredients of the given recipe</param>
    /// <returns>Boolean of if the a ingredient in the searched for ingredient list exists in given ingredient list</returns>
    private bool ContainsIngredient(List<Ingredient> recipeIngredients)
    {
        foreach (Ingredient ingredient in recipeIngredients)
        {
            if (Ingredients.Contains(ingredient))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Filters through the given list of recipes depending on the ingredient
    /// </summary>
    /// <param name="recipes">A given list of recipes to filter</param>
    /// <returns>A filtered list of recipes if they have the looked for ingredient</returns>
    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        List<Recipe> recipesList = recipes.ToList();
        var query = recipesList
            .Where(recipe => ContainsIngredient(new List<Ingredient>(recipe.Ingredients.Select(measured => new Ingredient(measured.Ingredient)))));
        return query.AsQueryable();
    }

    public override string ToString()
    {
        return "Ingredients";
    }
}