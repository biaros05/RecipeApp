namespace filtering;
using recipes;
// this class will filter by keyword
public class FilterByKeyword : IFilterBy 
{
    // in name or description
    public FilterByKeyword(string keyword)
    {
        Keyword = keyword;
    }
    private string Keyword;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => recipe.Name.Contains(Keyword))
            .Where(recipe => recipe.Description.Contains(Keyword))
            .Distinct();
        return filteredRecipes.ToList();
    }
}