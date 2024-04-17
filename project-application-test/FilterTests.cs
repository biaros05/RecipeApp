using filtering;
using recipes;
using users;

namespace project_application_test;
[TestClass]
public class FilterTests
{
    //tests for filter by time
    //tests for filter by time if time null
    //tests for filter by keyword tested name only, description only, both and none
    [TestMethod]
    public void FilterByKeyword_test()
    {
        //creating test recipes
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
        List<Recipe> recipes = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "recipe Test that is over 10 characters", new User("Bianca", "Rossetti"), "Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "recipe that is over 10 characters", new User("Bianca", "Rossetti"), "Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2)
            };

        //creating expected results
        List<Recipe> expected = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "recipe Test that is over 10 characters", new User("Bianca", "Rossetti"), "Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2)
            };

        IFilterBy filter = new FilterByKeyword("Test");
        List<Recipe> filteredRecipes = filter.FilterRecipes(recipes);

        CollectionAssert.AreEqual(expected, filteredRecipes);
    }

    //tests for filter by keyword if keyword is null
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByKeyword_Null_test()
    {
        IFilterBy filter = new FilterByKeyword("");
    }

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
        recipes.Add(new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2));
        recipes.Add(new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2));

        // creating expected recipes
        List<Recipe> expectedRecipes = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2)
            };

        // calling filtering
        IFilterBy filter = new FilterByIngredients(lookingForIngredient);
        List<Recipe> filteredRecipes = filter.FilterRecipes(recipes);


        CollectionAssert.AreEqual(expectedRecipes, filteredRecipes);
    }

    //tests for filter by ingredients if ingredients empty
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByIngredientsTestEmpty()
    {
        List<Ingredient> lookingForIngredient = new();
        IFilterBy filter = new FilterByIngredients(lookingForIngredient);
    }

    //tests for filter by rating
    [TestMethod]
    public void FilterByRatingTest()
    {
        //creating test data
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
        List<Recipe> recipes = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Not Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2)
            };
        //rate the first recipe 3 star and second 0
        recipes[0].RateRecipe(3);
        recipes[1].RateRecipe(1);

        //create expected results
        List<Recipe> expected = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
            };
        
        IFilterBy filter = new FilterByRating(3);
        List<Recipe> actual = filter.FilterRecipes(recipes);
        //assert
        CollectionAssert.AreEqual(expected, actual);
        
    }
    //tests for filter by rating if rating is less than 0
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByRatingZeroTest()
    {
        IFilterBy filter = new FilterByRating(0);
    }

    //test for filter by rating if rating is greater than 5
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByRatingMoreThanFiveTest()
    {
        IFilterBy filter = new FilterByRating(6);
    }

    //tests for filter by tags
    //tests for filter by tags if tags is empty
    //tests for filter by owner
    [TestMethod]
    public void FilterByOwnerTest()
    {
        // creating test data
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
        List<Recipe> recipes = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Not Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2)
            };
        
        // creating expected data
        List<Recipe> expected = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
            };
        
        IFilterBy filter = new FilterByOwner(new User("Biance", "Rossetti"));
        List<Recipe> filteredRecipes = filter.FilterRecipes(recipes);

        CollectionAssert.AreEqual(expected, filteredRecipes);
    }

    //tests for filter by owner if the owner does not exist
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByOwnerTestNull()
    {
        IFilterBy filter = new FilterByOwner(null);
    }

    //tests for filter by servings
    [TestMethod]
    public void FilterByServingTest()
    {
        //creating test data
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
        List<Recipe> recipes = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Not Bianca", "Rossetti"), "Test Description", 30, 60, 2,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Not Bianca", "Rossetti"), "Test Description", 30, 60, 5,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2)
            };
        
        //creating expected data
        List<Recipe> expected = new()
            {
                new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2),
                new(1, "Recipe need 10 characters", new User("Not Bianca", "Rossetti"), "Test Description", 30, 60, 5,
                new List<string> { "Step 1", "Step 2" }, dict2, new List<string> { "Tag1", "Tag2" }, 2)
            };
        
        //filter
        IFilterBy filter = new FilterByServings(3, 6);
        List<Recipe> actual = filter.FilterRecipes(recipes);

        CollectionAssert.AreEqual(expected, actual);

    }

    //tests for filter by servings if servings is less than 0
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByServingLessThanZeroTest()
    {
        IFilterBy filter = new FilterByServings(0, 5);
    }

    //test for filter by servering if min servings is bigger than max
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByServingMinBiggerThanMaxTest()
    {
        IFilterBy filter = new FilterByServings(4, 2);
    }

}