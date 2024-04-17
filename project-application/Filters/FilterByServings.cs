namespace filtering;
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
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => {
                if (recipe.NumberOfServings >= MinServings && recipe.NumberOfServings <= MaxServings)
                {
                    return true;
                }
                return false;
                });   
        return filteredRecipes.ToList();
    }
}