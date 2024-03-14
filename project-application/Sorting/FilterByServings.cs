namespace sorting;
using recipes;

public class FilterByServings : IFilterBy 
{
    // in name or description
    private int MinServings;
    private int MaxServings;
    // filters
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        throw new NotImplementedException();
    }
}