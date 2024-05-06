namespace filtering;

using System.Security.Cryptography;
using recipes;
public class FilterByRating : IFilterBy
{
    public FilterByRating(int rating)
    {
        if (rating <= 0 || rating > 5)
        {
            throw new InvalidOperationException("rating must not be negative or zero or bigger than 5");
        }
        Rating = rating;
    }

    private double? GetRecipeRating(Recipe r)
    {
        return r.Rating;
    }

    private int Rating;
    /// <summary>
    /// Filters a given list of recipes depending on the rating given
    /// </summary>
    /// <param name="recipes">A given list of recipes to filter</param>
    /// <returns>A filtered list of recipes that matches the given star rating</returns>
    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        List<Recipe> recipesList = recipes.ToList();
        var query = recipesList
            .Where(recipe => GetRecipeRating(recipe) >= Rating);

        return query.AsQueryable<Recipe>();

    }

    public override string ToString()
    {
        return "Ratings";
    }
}