namespace filtering;
using recipes;
using users;

public class FilterByOwner : IFilterBy 
{
    public FilterByOwner(User owner)
    {
        Owner = owner ?? throw new InvalidOperationException("owner cannot be null");
    }
    private User Owner;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => recipe.Owner.Equals(Owner));
        return filteredRecipes.ToList();
    }
}