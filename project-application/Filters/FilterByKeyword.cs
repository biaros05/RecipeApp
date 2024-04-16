namespace filtering;
using recipes;
// this class will filter by keyword
public class FilterByKeyword : IFilterBy 
{
    // using set to intersect then .IsEmpty() on it
    // in name or description
    private string Keyword;
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        throw new NotImplementedException();
    }
}