namespace sorting;
using recipes;
public interface IFilterBy {
    // returns sublist of recipes that match filter
    List<Recipe> FilterRecipes(List<Recipe> recipes);
}