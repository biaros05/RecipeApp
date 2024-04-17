namespace filtering;
using recipes;
public class FilterByRating : IFilterBy 
{
    public FilterByRating(int rating)
    {
        if (rating <= 0 || rating > 5)
        {
            throw new InvalidOperationException("rating must not be negative or bigger than 5");    
        }
        Rating = rating;
    }
    private int Rating;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => recipe.Rating >= Rating);
        return filteredRecipes.ToList();
    }
}