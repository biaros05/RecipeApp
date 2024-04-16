namespace filtering;
using recipes;
// this class will filter by ingredients
public class FilterByIngredients : IFilterBy 
{
    public FilterByIngredients(List<Ingredient> ingredients)
    {
        if (ingredients.Count() == 0 || ingredients == null)
        {
            throw new InvalidOperationException("ingredient list is null or empty");
        }
        Ingredients = ingredients;
    }
    private List<Ingredient> Ingredients;

    private bool ContainsIngredient(List<Ingredient> recipeIngredients)
    {
        foreach (Ingredient ingredient in recipeIngredients)
        {
            if (Ingredients.Contains(ingredient))
            {
                return true;
            }
        }   
        return false;
    }
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .TakeWhile(recipe => ContainsIngredient(new List<Ingredient>(recipe.Ingredients.Keys)));
        return filteredRecipes.ToList();
    }
}