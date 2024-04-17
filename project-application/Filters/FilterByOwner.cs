namespace filtering;
using recipes;
using users;

public class FilterByOwner : IFilterBy 
{
    public FilterByOwner(User owner)
    {
        if (owner == null) throw new ArgumentNullException("owner cannot be null");
        Owner = owner;
    }
    private User Owner;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => recipe.Owner.Equals(Owner));
        return filteredRecipes.ToList();
    }
}