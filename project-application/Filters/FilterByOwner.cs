namespace filtering;
using recipes;
using users;

public class FilterByOwner : IFilterBy 
{
    private User Owner;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        throw new NotImplementedException();
    }
}