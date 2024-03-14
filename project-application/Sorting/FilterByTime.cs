namespace sorting;
using recipes;

public class FilterByTime : IFilterBy 
{
    // in name or description
    private int MinTimeMins;
    private int MaxTimeMins;
    // filters
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        throw new NotImplementedException();
    }
}