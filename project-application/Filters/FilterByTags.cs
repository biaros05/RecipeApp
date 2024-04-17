namespace filtering;
using recipes;

public class FilterByTags : IFilterBy 
{
    public FilterByTags(List<string> tags)
    {
        if ((tags == null) || tags.Count == 0)
        {
            throw new InvalidOperationException("tags cannot be null or empty");
        }
        Tags = tags;
    }
    private List<string> Tags;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => !recipe.Tags.Intersect(Tags).Any());
        return filteredRecipes.ToList();
    }
}