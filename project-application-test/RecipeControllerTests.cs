namespace project_application_test;

using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using recipes;
using users;

[TestClass]
public class RecipeControllerTests
{
    // cleanup static members after every test
    [TestCleanup]
    public void TestCleanup()
    {
        RecipeController.AllRecipes = new List<Recipe>();
        RecipeController.Filters = new();
        RecipeController.Ingredients = new();
    }

    // TESTS FOR CREATERECIPE
    [TestMethod]
    public void CreateRecipe_ValidRecipe_ReturnsTrue()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(1, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        List<Recipe> expectedList = new(){new(1, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2)};
        List<Ingredient> expectedIngredients = new(){i};

        RecipeController.CreateRecipe(recipe);

        CollectionAssert.AreEqual(expectedList, RecipeController.AllRecipes);
        CollectionAssert.AreEqual(expectedIngredients, RecipeController.Ingredients);
    }

    [TestMethod]
    public void CreateRecipe_AlreadyExists_ThrowsException()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(1, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        RecipeController.CreateRecipe(recipe);

        Action act = () => RecipeController.CreateRecipe(recipe);

        Assert.ThrowsException<ArgumentException>(act, "Cannot add existing recipe");
    }

    [TestMethod]
    public void CreateRecipe_SimilarFieldsSameId_ThrowsException()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(1, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        RecipeController.CreateRecipe(recipe);

        Action act = () => RecipeController.CreateRecipe(new(1, "New Recipe", UserController.ActiveUser, "New Description", 40, 70, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "NewTag2" }, 2));

        Assert.ThrowsException<ArgumentException>(act, "Cannot add recipe with same ID");
    }

    [TestMethod]
    public void CreateRecipe_SimilarFieldsSameName_ThrowsException()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(2, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        RecipeController.CreateRecipe(recipe);

        Action act = () => RecipeController.CreateRecipe(new(1, "Test Recipe", UserController.ActiveUser, "New Description", 40, 70, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "NewTag2" }, 2));

        Assert.ThrowsException<ArgumentException>(act, "Cannot add recipe with same name");
    }

    [TestMethod]
    public void CreateRecipe_WrongOwner_ThrowsException()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(2, "Test Recipe", new User("Blob", "Blob"), "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        
        Action act = () => RecipeController.CreateRecipe(recipe);

        Assert.ThrowsException<ArgumentException>(act, "Cannot add recipe you arent the owner of");
    }

    // TESTS FOR DELETERECIPE
    [TestMethod]
    public void DeleteRecipe_RecipeDeleted_ReturnsTrue()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(2, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        Recipe recipe2 = new(1, "New Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        RecipeController.CreateRecipe(recipe);
        RecipeController.CreateRecipe(recipe2);
        List<Recipe> expectedRecipes = new(){recipe2};

        RecipeController.DeleteRecipe(recipe);

        CollectionAssert.AreEqual(expectedRecipes, RecipeController.AllRecipes);
    }

    [TestMethod]
    public void DeleteRecipe_RecipeDoesNotExist_ThrowsException()
    {
        UserController.ActiveUser = new User("Bianca", "Rossetti");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(2, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        Recipe recipe2 = new(1, "New Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        RecipeController.CreateRecipe(recipe);

        Action act = () => RecipeController.DeleteRecipe(recipe2);

        Assert.ThrowsException<ArgumentException>(act, "Cannot delete non-existent recipe");
    }

    [TestMethod]
    public void DeleteRecipe_IncorrectOwner_ThrowsException()
    {
        UserController.ActiveUser = new User("Blob", "Blob");
        Ingredient i = new("egg", Units.Quantity);
        Dictionary<Ingredient, double> dict = new();
        dict.Add(i, 20);
        Recipe recipe = new(2, "Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
            new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        RecipeController.CreateRecipe(recipe);
        UserController.ActiveUser = new User("Bianca", "Rossetti");

        Action act = () => RecipeController.DeleteRecipe(recipe);

        Assert.ThrowsException<ArgumentException>(act, "Cannot delete recipe you are not the owner of");
    }
}