namespace filtering;
using recipes;
using System;
using System.Linq;

public class FilterByTime : IFilterBy
{
    public FilterByTime(int min, int max)
    {
        if (min <= 0 || max > 420 || max < min)
        {
            throw new InvalidOperationException("Time must be between 0 and 420 inclusive and minimum must be less or equal to max");
        }
        MinTimeMins = min;
        MaxTimeMins = max;
        value = "min: " + min.ToString() + ", max: " + max.ToString();
    }
    private int MinTimeMins;
    private int MaxTimeMins;

    private string value;
    public string Value { get => value; set => this.value = value; }

    public string Name => "Time";

    /// <summary>
    /// Filters a given list of recipes using the specified min and max time
    /// </summary>
    /// <param name="recipes">A given list of recipes to filter</param>
    /// <returns>returns a filtered list of recipes that are above the min time and under the max time</returns>
    public IQueryable<Recipe>? FilterRecipes(IQueryable<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => (recipe.CookTimeMins + recipe.PrepTimeMins) >= MinTimeMins && (recipe.CookTimeMins + recipe.PrepTimeMins) <= MaxTimeMins);
        return filteredRecipes;
    }

    public override string ToString()
    {
        return "Time";
    }
}