namespace filtering;
using recipes;

public class FilterByTime : IFilterBy 
{
    public FilterByTime(int min, int max)
    {
        if (min < 0 || max > 420 || max < min)
        {
            throw new InvalidOperationException("Time must be between 0 and 420 inclusive and minimum must be less or equal to max");    
        }
        MinTimeMins = min;
        MaxTimeMins = max;
    }
    private int MinTimeMins;
    private int MaxTimeMins;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => {
                if (recipe.TotalTimeMins >= MinTimeMins && recipe.TotalTimeMins <= MaxTimeMins)
                {
                    return true;
                }
                return false;
            });
        return filteredRecipes.ToList();
    }
}