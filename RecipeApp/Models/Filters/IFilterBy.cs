namespace filtering;

using System.Linq;
using recipes;
public interface IFilterBy
{
    // returns sublist of recipes that match filter
    IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes);
}