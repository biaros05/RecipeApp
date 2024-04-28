namespace filtering;
using recipes;
public interface IFilterBy
{
    // returns sublist of recipes that match filter
    void FilterRecipes(IQueryable<Recipe> recipes);
}