namespace filtering;
using recipes;

public class FilterByTags : IFilterBy 
{
    // using set to intersect then .IsEmpty() on it
    private List<string> Tags;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        throw new NotImplementedException();
    }
}