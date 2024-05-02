namespace filtering;
using recipes;

public class FilterByTags : IFilterBy
{
    public FilterByTags(List<Tag> tags)
    {
        if ((tags == null) || tags.Count == 0)
        {
            throw new InvalidOperationException("tags cannot be null or empty");
        }
        Tags = tags;
    }
    private List<Tag> Tags;
    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        return recipes
            .Where(recipe => recipe.Tags.Intersect(Tags).Any());
    }

    public override string ToString()
    {
        return "Tags";
    }
}