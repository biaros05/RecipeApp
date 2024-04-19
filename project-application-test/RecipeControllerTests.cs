namespace project_application_test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using recipes;
using users;

[TestClass]
public class RecipeControllerTests
{
    // cleanup static members after every test

    // TESTS FOR CREATERECIPE
    // [TestMethod]
    // public void CreateRecipe_ValidRecipe_ReturnsTrue()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     List<Recipe> expectedList = new(){new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2)};
    //     List<Ingredient> expectedIngredients = new(){i};

    //     instance.CreateRecipe(recipe);

    //     CollectionAssert.AreEqual(expectedList, instance.AllRecipes);
    //     CollectionAssert.AreEqual(expectedIngredients, instance.Ingredients);
    // }

    // [TestMethod]
    // public void CreateRecipe_AlreadyExists_ThrowsException()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     instance.CreateRecipe(recipe);

    //     Action act = () => instance.CreateRecipe(recipe);

    //     Assert.ThrowsException<ArgumentException>(act, "Cannot add existing recipe");
    // }

    // [TestMethod]
    // public void CreateRecipe_SimilarFieldsSameId_ThrowsException()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     instance.CreateRecipe(recipe);

    //     Action act = () => instance.CreateRecipe(new("New Recipe", UserController.ActiveUser, "New Description", 40, 70, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "NewTag2" }, 2));

    //     Assert.ThrowsException<ArgumentException>(act, "Cannot add recipe with same ID");
    // }

    // [TestMethod]
    // public void CreateRecipe_SimilarFieldsSameName_ThrowsException()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     instance.CreateRecipe(recipe);

    //     Action act = () => instance.CreateRecipe(new("Test Recipe", UserController.ActiveUser, "New Description", 40, 70, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "NewTag2" }, 2));

    //     Assert.ThrowsException<ArgumentException>(act, "Cannot add recipe with same name");
    // }

    // [TestMethod]
    // public void CreateRecipe_WrongOwner_ThrowsException()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", new User("Blob", "Blob"), "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
        
    //     Action act = () => instance.CreateRecipe(recipe);

    //     Assert.ThrowsException<ArgumentException>(act, "Cannot add recipe you arent the owner of");
    // }

    // // TESTS FOR DELETERECIPE
    // [TestMethod]
    // public void DeleteRecipe_RecipeDeleted_ReturnsTrue()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     Recipe recipe2 = new("New Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     instance.CreateRecipe(recipe);
    //     instance.CreateRecipe(recipe2);
    //     List<Recipe> expectedRecipes = new(){recipe2};

    //     instance.DeleteRecipe(recipe);

    //     CollectionAssert.AreEqual(expectedRecipes, instance.AllRecipes);
    // }
    
    // [TestMethod]
    // public void DeleteRecipe_RecipeDoesNotExist_ThrowsException()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     Recipe recipe2 = new("New Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     instance.CreateRecipe(recipe);

    //     Action act = () => instance.DeleteRecipe(recipe2);

    //     Assert.ThrowsException<ArgumentException>(act, "Cannot delete non-existent recipe");
    // }

    // [TestMethod]
    // public void DeleteRecipe_IncorrectOwner_ThrowsException()
    // {
    //     RecipeController instance = RecipeController.Instance;
    //     UserController.ActiveUser = new User("Blob", "Blob");
    //     Ingredient i = new("egg", Units.Quantity);
    //     Dictionary<Ingredient, double> dict = new();
    //     dict.Add(i, 20);
    //     Recipe recipe = new("Test Recipe", UserController.ActiveUser, "Test Description", 30, 60, 4,
    //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
    //     instance.CreateRecipe(recipe);
    //     UserController.ActiveUser = new User("Bianca", "Rossetti");

    //     Action act = () => instance.DeleteRecipe(recipe);

    //     Assert.ThrowsException<ArgumentException>(act, "Cannot delete recipe you are not the owner of");
    // }
}