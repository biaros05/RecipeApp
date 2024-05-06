namespace project_application_test;

using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using filtering;
using Microsoft.EntityFrameworkCore;
using Moq;
using recipes;
using users;

[TestClass]
public class RecipesTest
{
    // private static (Mock<RecipesContext>, Mock<DbSet<Recipe>>) GetMocks()
    // {
    //     var mockContext = new Mock<RecipesContext>();
    //     var mockRecipes = new Mock<DbSet<Recipe>>();
    //     mockContext.Setup(mock => mock.RecipeManager_Recipes).Returns(mockRecipes.Object);

    //     return (mockContext, mockRecipes);
    // }
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

    [TestCleanup()]
    public void Cleanup()
    {
        RecipesContext.Instance = null;
        RecipeController.Instance = null;
    }
    // TESTS FOR NAME PROPERTY
    [TestMethod]
    public void Name_SetValidName_SetsNameCorrectly()
    {
        // Arrange
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        string validName = "Delicious Recipe";

        // Act
        recipe.Name = validName;

        // Assert
        Assert.AreEqual(validName, recipe.Name);
    }

    [TestMethod]
    public void Name_SetNullName_ThrowsArgumentNullException()
    {
        // Arrange
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.Name = null;

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(act, "Null name should throw exception.");
    }

    [TestMethod]
    public void Name_SetShortName_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        string shortName = "Short";

        Action act = () => recipe.Name = shortName;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Name under 10 chars should throw exception");
    }

    [TestMethod]
    public void Name_SetLongName_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        string longName = "ThisIsAVeryLongRecipeNameThatExceedsTheMaximumAllowedLengthThisIsAVeryLongRecipeNameThatExceedsTheMaximumAllowedLength";

        Action act = () => recipe.Name = longName;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Name over 100 chars should throw exception");
    }

    // TESTS FOR DESCRIPTION
    [TestMethod]
    public void Description_SetValidDescription_SetsDescription_SetsDescriptionCorrectly()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        string validDescription = "This is a valid description";

        recipe.Description = validDescription;

        Assert.AreEqual(validDescription, recipe.Description);
    }

    [TestMethod]
    public void Description_SetEmptyDescription_SetsEmptyDescription_SetsDescriptionAsRecipeName()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        string EmptyDescription = "";

        recipe.Description = EmptyDescription;

        Assert.AreEqual("Test Recipe", recipe.Description);
    }

    [TestMethod]
    public void Description_SetNullDescription_SetsDescriptionAsRecipeName()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        recipe.Description = null;

        Assert.AreEqual("Test Recipe", recipe.Description);
    }

    // TESTS FOR PREPTIMEMINS PROPERTY
    [TestMethod]
    public void PrepTimeMins_SetPrepTimeCorrectly_ReturnsTrue()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        recipe.PrepTimeMins = 30;

        Assert.AreEqual(30, recipe.PrepTimeMins);
    }

    [TestMethod]
    public void PrepTimeMins_SetPrepTimeGreaterThan4_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.PrepTimeMins = 2400;

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be greater than 4 hours");
    }

    [TestMethod]
    public void PrepTimeMins_SetPrepTimeNegative_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.PrepTimeMins = -100;

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be negative");
    }

    // TESTS FOR COOKTIMEMINS PROPERTY
    [TestMethod]
    public void CookTimeMins_SetCookTimeCorrectly_ReturnsTrue()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        recipe.CookTimeMins = 30;

        Assert.AreEqual(30, recipe.CookTimeMins);
    }

    [TestMethod]
    public void CookTimeMins_SetCookTimeGreaterThan4_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.CookTimeMins = 2400;

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be greater than 4 hours");
    }

    [TestMethod]
    public void CookTimeMins_SetCookTimeNegative_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.CookTimeMins = -100;

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be negative");
    }

    // TEST TOTALTIMEMINS PROPERTY
    [TestMethod]
    public void TotalTimeMins_GetTotalTime_ReturnsTrue()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        string totalTime = recipe.TotalTime;

        Assert.AreEqual("1h30mins", totalTime);
    }

    // TEST FOR RATING & RATERECIPE
    [TestMethod]
    public void RateRecipe_AddValidRating_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        Recipe r = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            r
        }.AsQueryable();

        // act 
        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);


        r.RateRecipe(4, UserController.Instance.ActiveUser);

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
        {
            _ratings = new List<Rating>{
            new(4, UserController.Instance.ActiveUser)
        }
        };
        Assert.AreEqual(4, r.Rating);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void RateRecipe_AddSeveralRatings_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        Recipe r = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            r
        }.AsQueryable();

        // act 
        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        var mockSetRatings = new Mock<DbSet<Rating>>();
        mockContext.Setup(m => m.RecipeManager_Ratings).Returns(mockSetRatings.Object);

        r.RateRecipe(4, UserController.Instance.ActiveUser);
        r.RateRecipe(5, new User("user1", "123456789"));
        r.RateRecipe(5, new User("user2", "123456789"));
        r.RateRecipe(3, new User("user3", "123456789"));

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
        {
            _ratings = new List<Rating>{
            new(4, UserController.Instance.ActiveUser),
            new(5, new User("user1", "123456789")),
            new(5, new User("user2", "123456789")),
            new(3, new User("user3", "123456789"))
        }
        };
        Assert.AreEqual(4.25, r.Rating);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Exactly(4));
    }

    [TestMethod]
    public void RateRecipe_RateLessThan1_ThrowsArgumentException()
    {
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        var recipes = new List<Recipe>
        {
            new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
        }.AsQueryable();

        // act 
        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        Action act = () => RecipesContext.Instance.RecipeManager_Recipes.First().RateRecipe(-4, new User("Bianca", "123456789"));

        
        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be less than 1");
    }

    [TestMethod]
    public void RateRecipe_GreaterThan5_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.RateRecipe(6, new User("Bianca", "123456789"));

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be greater than 5");
    }

    // TESTING FOR DIFFICULTY AND RATE DIFFICULTY
    [TestMethod]
    public void RateDifficulty_AddValidRating_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();
         //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        Recipe r = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            r
        }.AsQueryable();

        // act 
        RecipesContext.Instance = mockContext.Object;
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        r.RateDifficulty(4, new User("Bianca", "123456789"));

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
        {
            _difficulties = new List<DifficultyRating>{
            new(4, UserController.Instance.ActiveUser)
        }
        };
        
        Assert.AreEqual(r.DifficultyRating, 4);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void RateDifficulty_AddSeveralRatings_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        Recipe r = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            r
        }.AsQueryable();

        // act 
        RecipesContext.Instance = mockContext.Object;
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        r.RateDifficulty(4, UserController.Instance.ActiveUser);
        r.RateDifficulty(6, new User("user1", "123456789"));
        r.RateDifficulty(7, new User("user2", "123456789"));
        r.RateDifficulty(3, new User("user3", "123456789"));

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
        {
            _difficulties = new List<DifficultyRating>{
            new(4, UserController.Instance.ActiveUser),
            new(6, new User("user1", "123456789")),
            new(7, new User("user2", "123456789")),
            new(3, new User("user3", "123456789"))
        }
        };
        Assert.AreEqual(5, r.DifficultyRating);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Exactly(4));
    }

    [TestMethod]
    public void RateDifficulty_RateLessThan1_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.RateDifficulty(-4, new User("Bianca", "123456789"));

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Difficulty rating cannot be less than 1");
    }

    [TestMethod]
    public void RateDifficulty_GreaterThan10_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        Action act = () => recipe.RateDifficulty(11, new User("Bianca", "123456789"));

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be greater than 10");
    }

    // TESTS FOR ADDTAG METHOD
    [TestMethod]
    public void AddTag_CorrectTag_ReturnsTrue()
    {
        var mockContext = new Mock<RecipesContext>();
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        var recipes = new List<Recipe>
        {
            new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
        }.AsQueryable();

        // act 
        RecipesContext.Instance = mockContext.Object;
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        RecipesContext.Instance.RecipeManager_Recipes.First().AddTag("school lunch");

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2"), new("school lunch") }, 2);

        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void AddTag_ExistingTag_DoesNotAddTag()
    {
        var mockContext = new Mock<RecipesContext>();
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));

        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        List<Tag> correctTags = new() { new("Tag1"), new("Tag2") };
        var recipes = new List<Recipe>(){recipe}.AsQueryable();
        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        recipe.AddTag("Tag1");

        CollectionAssert.AreEqual(correctTags, recipe.Tags);
    }

    [TestMethod]
    public void AddTag_EmptyTag_ThrowsArgumentException()
    {
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        List<string> correctTags = new() { "Tag1", "Tag2" };

        Action act = () => recipe.AddTag("");

        Assert.ThrowsException<ArgumentException>(act, "Cannot add empty tag");
    }

    // TESTING UPDATERECIPE METHOD
    [TestMethod]
    public void UpdateRecipe_CorrectParameters_UpdatesRecipesCorrectly()
    {
        var mockContext = new Mock<RecipesContext>();
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        var ing = new List<Ingredient>()
        {
            i
        }.AsQueryable();
        Recipe r = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            r
        }.AsQueryable();

        string newDescription = "Updated Description";
        int newPrepTime = 45;
        int newCookTime = 75;
        List<MeasuredIngredient> newIngredients = new()
        {
            new(new Ingredient("flour", Units.Mass), 300),
            new(new Ingredient("egg", Units.Quantity), 4)
        };
        List<Tag> newTags = new List<Tag> { new("NewTag1"), new("NewTag2") };

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Updated Description", 45, 75, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, newIngredients, newTags, 2);

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        r.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);

        mockSetRecipe.Verify(mock => mock.Update(It.Is<Recipe>(
            actual => expected.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.AtLeastOnce);
    }

    [TestMethod]
    public void UpdateRecipe_EmptyDescription_UpdatesRecipesCorrectly()
    {
        var mockContext = new Mock<RecipesContext>();
        //test data
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        var ingredients = new List<Ingredient>(){i}.AsQueryable();
        List<MeasuredIngredient> dict = new()
        {
            new(i, 20)
        };
        var ing = new List<Ingredient>()
        {
            i
        }.AsQueryable();
        Recipe r = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            r
        }.AsQueryable();

        // act 

        string newDescription = "";
        int newPrepTime = 45;
        int newCookTime = 75;
        List<MeasuredIngredient> newIngredients = new()
        {
            new(new Ingredient("flour", Units.Mass), 300),
            new(new Ingredient("egg", Units.Quantity), 4)
        };
        List<Tag> newTags = new List<Tag> { new("NewTag1"), new("NewTag2") };

        //expected data
        Recipe expected = new Recipe("Test Recipe", UserController.Instance.ActiveUser, "", 45, 75, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, newIngredients, newTags, 2);


        //finalize context
        RecipesContext.Instance = mockContext.Object;

        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        ConfigureDbSetMock(ingredients, mockSetIngredient);
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        r.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);

        mockSetRecipe.Verify(mock => mock.Update(It.Is<Recipe>(
            actual => expected.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.AtLeastOnce);
    }

    [TestMethod]
    public void UpdateRecipe_NegativePrepTime_ThrowsException()
    {
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        string newDescription = "Description";
        int newPrepTime = -12;
        int newCookTime = 75;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Tag> newTags = new List<Tag> { new("NewTag1"), new("NewTag2") };

        Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Prep time cannot be negative");
    }

    [TestMethod]
    public void UpdateRecipe_PrepTimeTooLarge_ThrowsException()
    {
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        string newDescription = "Description";
        int newPrepTime = 500;
        int newCookTime = 75;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Tag> newTags = new List<Tag> { new("NewTag1"), new("NewTag2") };

        Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Prep time cannot be greather than 4 hours");
    }

    [TestMethod]
    public void UpdateRecipe_CookTimeTooLarge_ThrowsException()
    {
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        string newDescription = "Description";
        int newPrepTime = 30;
        int newCookTime = 750;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Tag> newTags = new List<Tag> { new("NewTag1"), new("NewTag2") };

        Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Cook time cannot be greater than 4 hours");
    }

    [TestMethod]
    public void UpdateRecipe_CookTimeNegative_ThrowsException()
    {
        UserController.Instance.ActiveUser = new User("Bianca", "123456789");
        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);

        string newDescription = "Description";
        int newPrepTime = 30;
        int newCookTime = -10;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Tag> newTags = new List<Tag> { new("NewTag1"), new("NewTag2") };

        Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);

        Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Cook time cannot be negative");
    }

    // CONSTRUCTOR TESTS:
    [TestMethod]
    public void Constructor_ValidParams_InitializesCorrectly()
    {
        string name = "Test Recipe";
        User owner = new User("Bianca", "123456789");
        string newDescription = "Test Description";
        int newPrepTime = 30;
        int newCookTime = 60;
        int numServings = 4;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Instruction> instructions = new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") };
        List<Tag> newTags = new List<Tag> { new("Tag1"), new("Tag2") };
        int budget = 2;

        Recipe recipe = new(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);

        Assert.AreEqual(name, recipe.Name);
        Assert.AreEqual(owner, recipe.Owner);
        Assert.AreEqual(newDescription, recipe.Description);
        Assert.AreEqual(newPrepTime, recipe.PrepTimeMins);
        Assert.AreEqual(newCookTime, recipe.CookTimeMins);
        Assert.AreEqual(numServings, recipe.NumberOfServings);
        CollectionAssert.AreEqual(instructions, recipe.Instructions);
        CollectionAssert.AreEqual(newIngredients, recipe.Ingredients);
        CollectionAssert.AreEqual(newTags, recipe.Tags);
        Assert.AreEqual("$$", recipe.Budget);
    }

    [TestMethod]
    public void Constructor_EmptyInstructions_ThrowsException()
    {
        string name = "Test Recipe";
        User owner = new User("Bianca", "123456789");
        string newDescription = "Test Description";
        int newPrepTime = 30;
        int newCookTime = 60;
        int numServings = 4;

        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Instruction> instructions = new();

        List<Tag> newTags = new List<Tag> { new("Tag1"), new("Tag2") };
        int budget = 2;

        Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);

        Assert.ThrowsException<ArgumentException>(act, "must contain instructions");
    }

    [TestMethod]
    public void Constructor_EmptyIngredients_ThrowsException()
    {
        string name = "Test Recipe";
        User owner = new User("Bianca", "123456789");
        string newDescription = "Test Description";
        int newPrepTime = 30;
        int newCookTime = 60;
        int numServings = 4;
        List<MeasuredIngredient> newIngredients = new();
        List<Instruction> instructions = new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") };
        List<Tag> newTags = new List<Tag> { new("Tag1"), new("Tag2") };
        int budget = 2;

        Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);

        Assert.ThrowsException<ArgumentException>(act, "must contain ingredients");
    }

    [TestMethod]
    public void Constructor_InvalidBudgetOver3_ThrowsException()
    {
        string name = "Test Recipe";
        User owner = new User("Bianca", "123456789");
        string newDescription = "Test Description";
        int newPrepTime = 30;
        int newCookTime = 60;
        int numServings = 4;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Instruction> instructions = new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") };
        List<Tag> newTags = new List<Tag> { new("Tag1"), new("Tag2") };
        int budget = 4;

        Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);

        Assert.ThrowsException<ArgumentException>(act, "budget cannot be greater than 3");
    }

    [TestMethod]
    public void Constructor_InvalidBudgetLessThan1_ThrowsException()
    {
        string name = "Test Recipe";
        User owner = new User("Bianca", "123456789");
        string newDescription = "Test Description";
        int newPrepTime = 30;
        int newCookTime = 60;
        int numServings = 4;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Instruction> instructions = new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") };
        List<Tag> newTags = new List<Tag> { new("Tag1"), new("Tag2") };
        int budget = 0;

        Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);

        Assert.ThrowsException<ArgumentException>(act, "budget cannot be greater than 3");
    }

    // COPY CONSTRUCTOR TESTS
    [TestMethod]
    public void CopyConstructor_ValidParams_InitializesCorrectly()
    {
        string name = "Test Recipe";
        User owner = new User("Bianca", "123456789");
        string newDescription = "Test Description";
        int newPrepTime = 30;
        int newCookTime = 60;
        int numServings = 4;
        List<MeasuredIngredient> newIngredients = new();
        newIngredients.Add(new(new Ingredient("flour", Units.Quantity), 300));
        List<Instruction> instructions = new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") };
        List<Tag> newTags = new List<Tag> { new("Tag1"), new("Tag2") };
        int budget = 2;
        Recipe recipe = new(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);

        Recipe copiedRecipe = new(recipe);

        Assert.AreEqual(name, copiedRecipe.Name);
        Assert.AreEqual(owner, copiedRecipe.Owner);
        Assert.AreEqual(newDescription, copiedRecipe.Description);
        Assert.AreEqual(newPrepTime, copiedRecipe.PrepTimeMins);
        Assert.AreEqual(newCookTime, copiedRecipe.CookTimeMins);
        Assert.AreEqual(numServings, copiedRecipe.NumberOfServings);
        CollectionAssert.AreEqual(instructions, copiedRecipe.Instructions);
        CollectionAssert.AreEqual(newIngredients, copiedRecipe.Ingredients);
        CollectionAssert.AreEqual(newTags, copiedRecipe.Tags);
        Assert.AreEqual("$$", copiedRecipe.Budget);
    }
}