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
    /// <summary>
    /// Filters the given recipes by if they are owned by the given owner
    /// </summary>
    /// <param name="recipes">A given list of recipes to filter</param>
    /// <returns>Returns a filtered list of recipes if the given user Owns that recipe</returns>
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => recipe.Owner.Equals(Owner));
        return filteredRecipes.ToList();
    }
}