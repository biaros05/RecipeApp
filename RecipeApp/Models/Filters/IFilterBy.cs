namespace filtering;

using System.Linq;
using recipes;
public interface IFilterBy
{
    // returns sublist of recipes that match filter
    public string Value { get; set; }
    public string Name { get;}
    IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes);
}