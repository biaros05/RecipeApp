namespace filtering;
using recipes;
using users;

public class FilterByOwner : IFilterBy 
{
    // in name or description
    private User Owner;
    // filters
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        throw new NotImplementedException();
    }
}