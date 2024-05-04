namespace filtering;
using recipes;
// this class will filter by keyword
public class FilterByKeyword : IFilterBy
{
    // in name or description
    public FilterByKeyword(string keyword)
    {
        if (keyword == null || keyword == "")
        {
            throw new InvalidOperationException("keyword must not be null or empty");
        }
        Keyword = keyword;
    }
    private string Keyword;
    /// <summary>
    /// Filteres the given recipes list depending on the keyword specified
    /// </summary>
    /// <param name="recipes">given list of recipes</param>
    /// <returns>returns a list of filtered recipes where the name or description contains the keyword</returns>
    /// <exception cref="InvalidOperationException">When the given recipe has a null or empty name or description</exception>
    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        return recipes
            //finds if the recipe name contains the keyword
            .Where(recipe => recipe.Name.Contains(Keyword) || recipe.Description.Contains(Keyword));
    }

    public override string ToString()
    {
        return "Keyword";
    }
}