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

    /// <summary>
    /// Taken a list of Ingredients, compares and returns true if one ingredient matches with one ingredient in given list
    /// </summary>
    /// <param name="recipeIngredients">Ingredients of the given recipe</param>
    /// <returns>Boolean of if the a ingredient in the searched for ingredient list exists in given ingredient list</returns>
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
    /// <summary>
    /// Filters through the given list of recipes depending on the ingredient
    /// </summary>
    /// <param name="recipes">A given list of recipes to filter</param>
    /// <returns>A filtered list of recipes if they have the looked for ingredient</returns>
    public List<Recipe> FilterRecipes(List<Recipe> recipes)
    {
        var filteredRecipes = recipes
            .Where(recipe => ContainsIngredient(new List<Ingredient>(recipe.Ingredients.Keys)));
        return filteredRecipes.ToList();
    }

    public override string ToString()
    {
        return "Ingredients";
    }
}