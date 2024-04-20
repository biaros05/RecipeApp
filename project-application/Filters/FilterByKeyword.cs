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
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            //finds if the recipe name contains the keyword
            .Where(recipe =>
            {
                if (recipe.Name == null || recipe.Name == "")
                {
                    throw new InvalidOperationException("Recipe name is null or empty");
                }
                bool result = recipe.Name.Contains(Keyword);
                if (result == false)
                {
                    if (recipe.Description == null || recipe.Description == "")
                    {
                        throw new InvalidOperationException("Recipe description is null or empty");
                    }
                    return recipe.Description.Contains(Keyword);
                }
                else
                {
                    return result;
                }
            });
        return filteredRecipes.ToList();
    }
}