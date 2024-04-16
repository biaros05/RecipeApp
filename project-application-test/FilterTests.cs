using filtering;
using recipes;
using users;

namespace project_application_test;
[TestClass]
public class FilterTests
{
    //tests for filter by time
    //tests for filter by time if time null
    //tests for filter by keyword
    //tests for filter by keyword if keyword is null
    //tests for filter by ingredients
    [TestMethod]
        public void FilterByIngredients_test()
        {
            // adding the ingredients we are looking for
            List<Ingredient> lookingForIngredient = new()
            {
                new("Apple", Units.Quantity),
                new("Banana", Units.Mass)
            };
            
            List<Recipe> recipes = new();
            // adding test recipes
            Ingredient a = new("Apple", Units.Quantity);
            Ingredient b = new("Sugar", Units.Mass);
            Dictionary<Ingredient, double> dict = new()
            {
                { a, 20 },
            };
            Dictionary<Ingredient, double> dict2 = new()
            {
                { b, 20 },
            };
            recipes.Add (new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2));
            recipes.Add (new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2));

            // creating expected recipes
            List<Recipe> expectedRecipes = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2)
            };

            // calling filterin
            IFilterBy filterByIngredient = new FilterByIngredients(lookingForIngredient);
            List<Recipe> filteredRecipes = filterByIngredient.FilterRecipes (recipes);

            
            CollectionAssert.AreEqual(expectedRecipes, filteredRecipes);
        }
    //tests for filter by ingredients if ingredients empty
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
        public void FilterByIngredients_test_empty()
        {
            List<Ingredient> lookingForIngredient = new();
            IFilterBy filterByIngredient = new FilterByIngredients(lookingForIngredient);
        }
    //tests for filter by rating
    //tests for filter by rating if rating is null
    //tests for filter by tags
    //tests for filter by tags if tags is empty
    //tests for filter by owner
    //tests for filter by owner if owner does not have recipe
    //tests for filter by servings
    //tests for filter by servings if servings is null
    

}