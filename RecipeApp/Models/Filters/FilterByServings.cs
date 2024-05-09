namespace filtering;

using System;
using System.Linq;
using recipes;

public class FilterByServings : IFilterBy
{
    public FilterByServings(int min, int max)
    {
        if ((min <= 0 || max < min))
        {
            throw new InvalidOperationException("minimum must be greater than zero and maximum must be greater than minimum");
        }
        MinServings = min;
        MaxServings = max;
    }
    private int MinServings;
    private int MaxServings;
    /// <summary>
    /// Filters a given list of recipes by its serving
    /// </summary>
    /// <param name="recipes">a given list of recipes to filter</param>
    /// <returns>a list of filtered recipes that fit into the minimum and maximum amount of serving</returns>
    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        return recipes
            .Where(recipe => recipe.NumberOfServings >= MinServings && recipe.NumberOfServings <= MaxServings);
    }

    public override string ToString()
    {
        return "Serving";
    }
}