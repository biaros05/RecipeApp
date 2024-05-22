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
        Value = "min: " + MinServings.ToString() + ", max: " + MaxServings.ToString();
    }
    private int MinServings;
    private int MaxServings;

    private string value;
    public string Value { get => value; set => this.value = value; }

    public string Name => "Servings";

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