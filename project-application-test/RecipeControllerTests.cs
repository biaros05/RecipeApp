namespace project_application_test;
using filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using recipes;
using users;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public class RecipeControllerTests
{
    [TestCleanup()]
    public void Cleanup()
    {
        RecipeController.Instance = null;
        RecipesContext.Instance = null;
    }

    private static void ConfigureDbSetMock<T>(
    IQueryable<T> data, Mock<DbSet<T>> mockDbSet) where T : class
    {
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.Provider)
        .Returns(data.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.Expression)
        .Returns(data.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.ElementType)
        .Returns(data.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(mock => mock.GetEnumerator())
        .Returns(data.GetEnumerator());
    }

    // FILTER RECIPE TESTS
    [TestMethod]
    public void FilterBy_MultipleFilters_FiltersCorrectly()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //creating expected data
        List<Recipe> expected = new()
            {
                new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            };

        IFilterBy filter = new FilterByServings(3, 6);
        IFilterBy filter2 = new FilterByKeyword("Test");
        RecipeController.Instance.AddFilter(filter);
        RecipeController.Instance.AddFilter(filter2);

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        
        // basic setup
        List<Recipe> filteredRecipes = RecipeController.Instance.FilterBy();

        CollectionAssert.AreEqual(expected, filteredRecipes);
    }
    
    [TestMethod]
    public void FilterBy_NoFilters_ReturnsAllRecipes()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //creating expected data
        List<Recipe> expected = new()
            {
                new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
            };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        
        // basic setup
        List<Recipe> filteredRecipes = RecipeController.Instance.FilterBy();

        CollectionAssert.AreEqual(expected, filteredRecipes);
    }

    
    [TestMethod]
    public void FilterBy_NoRecipes_ReturnsEmptyList()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
        }.AsQueryable();

        //creating expected data
        List<Recipe> expected = new()
            {
            };

        
        IFilterBy filter = new FilterByServings(3, 6);
        IFilterBy filter2 = new FilterByKeyword("Test");
        RecipeController.Instance.AddFilter(filter);
        RecipeController.Instance.AddFilter(filter2);

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        
        // basic setup
        List<Recipe> filteredRecipes = RecipeController.Instance.FilterBy();

        CollectionAssert.AreEqual(expected, filteredRecipes);
    }

    // TESTS FOR CREATERECIPE
    [TestMethod]
    public void CreateRecipe_ValidRecipe_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        IFilterBy filter = new FilterByServings(3, 6);
        IFilterBy filter2 = new FilterByKeyword("Test");
        RecipeController.Instance.AddFilter(filter);
        RecipeController.Instance.AddFilter(filter2);

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //creating expected data
        Recipe r = new("New created recipe!", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        RecipeController.CreateRecipe(r);

        mockSetRecipe.Verify(mock => mock.Add(It.Is<Recipe>(
            actual => r.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void CreateRecipe_NewIngredient_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();


        IFilterBy filter = new FilterByServings(3, 6);
        IFilterBy filter2 = new FilterByKeyword("Test");
        RecipeController.Instance.AddFilter(filter);
        RecipeController.Instance.AddFilter(filter2);

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // setup mock
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        List<MeasuredIngredient> newIngredient = new()
        {
            new ( new("Caramel", Units.Quantity), 20 )
        };

        //creating expected data
        Recipe r = new("New created recipe!", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, newIngredient, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        RecipeController.CreateRecipe(r);

        mockSetRecipe.Verify(mock => mock.Add(It.Is<Recipe>(
            actual => r.Equals(actual))), Times.Once);
        mockSetIngredient.Verify(mock => mock.Add(It.IsAny<Ingredient>()), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.AtLeastOnce);
    }

    [TestMethod]
    public void CreateRecipe_ExistingIngredient_DoesNotAdd()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        List<MeasuredIngredient> existingIngredient = new()
        {
            new ( new("Apple", Units.Quantity), 20 )
        };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // setup mock
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //creating expected data
        Recipe r = new("New created recipe!", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, existingIngredient, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        RecipeController.CreateRecipe(r);

        mockSetRecipe.Verify(mock => mock.Add(It.Is<Recipe>(
            actual => r.Equals(actual))), Times.Once);
        mockSetIngredient.Verify(mock => mock.Add(It.IsAny<Ingredient>()), Times.Never);
        mockContext.Verify(mock => mock.SaveChanges(), Times.AtLeastOnce);
    }


    [TestMethod]
    public void CreateRecipe_SameNameSameOwner_ThrowsException()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe",  UserController.Instance.ActiveUser, "Test Description2222", 50, 70, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //creating expected data
        Recipe r = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => RecipeController.CreateRecipe(r);

        Assert.ThrowsException<ArgumentException>(act, "Cannot add existing recipe");
    }

    
    [TestMethod]
    public void CreateRecipe_SameNameDiffOwner_WillAdd()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe",  UserController.Instance.ActiveUser, "Test Description2222", 50, 70, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        UserController.Instance.ActiveUser = new User("Bobby", "1234567890");
        //creating expected data
        Recipe r = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        // Act
        RecipeController.CreateRecipe(r);

        // Assert
        mockSetRecipe.Verify(mock => mock.Add(It.Is<Recipe>(
            actual => r.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void CreateRecipe_WrongUser_ThrowsException()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe",  UserController.Instance.ActiveUser, "Test Description2222", 50, 70, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //creating expected data
        Recipe r = new("Test Recipe", new User("Bobby", "1234567890"), "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        // Act
        Action act = () => RecipeController.CreateRecipe(r);

        // Assert
        Assert.ThrowsException<ArgumentException>(act, "Cannot create recipe for another user");
    }

    // TESTS FOR DELETERECIPE
    [TestMethod]
    public void DeleteRecipe_ValidRecipe_WillDelete()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        RecipeController.DeleteRecipe(new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2));

        mockSetRecipe.Verify(mock => mock.Remove(It.IsAny<Recipe>()), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void DeleteRecipe_WrongUser_ThrowsException()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe",  UserController.Instance.ActiveUser, "Test Description2222", 50, 70, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //creating expected data
        Recipe r = new("Test Recipe", new User("Bobby", "1234567890"), "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        // Act
        Action act = () => RecipeController.DeleteRecipe(r);

        // Assert
        Assert.ThrowsException<ArgumentException>(act, "Cannot delete recipe for another user");
    }

    [TestMethod]
    public void DeleteRecipe_RecipeDoesNotExist_ThrowsException()
    {
        var mockContext = new Mock<RecipesContext>();

        //creating test data
        UserController.Instance.ActiveUser = new User("Bianca", "Rossetti");

        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        var ingredients = new List<Ingredient>() {
            a, b
        }.AsQueryable();

        List<MeasuredIngredient> dict = new()
            {
                new ( a, 20 )
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new ( b, 20 )
            };

        var recipes = new List<Recipe>() {
            new("Test Recipe",  UserController.Instance.ActiveUser, "Test Description2222", 50, 70, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            new(new("Recipe need 10 characters", UserController.Instance.ActiveUser, "Description", 30, 60, 5,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2))
        }.AsQueryable();

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //creating expected data
        Recipe r = new("Blah Blah Blah", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
                new List<Instruction> { new(1, "Step 1"), new(2,"Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        // Act
        Action act = () => RecipeController.DeleteRecipe(r);

        // Assert
        Assert.ThrowsException<ArgumentException>(act, "Cannot delete recipe for another user");
    }
}