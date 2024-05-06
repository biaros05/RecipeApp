using System.Collections.ObjectModel;
using filtering;
using recipes;
using users;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace project_application_test;
[TestClass]
public class FilterTests
{
    [TestCleanup()]
    public void Cleanup()
    {
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


    [TestMethod]
    //tests for filter by time
    public void FilterByTimeTest()
    {
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        //creating test data
        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b =  new("Sugar", Units.Mass);

        List<MeasuredIngredient> dict = new()
            {
                new(a, 20),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new(b, 20 ),
            };
        List<Recipe> recipes = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 45, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"),new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 15, 15, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag2"), new("Tag3") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 60, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag4"), new("Tag6") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 90, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag4"), new("Tag1") }, 2)
            };

        //create expected data
        List<Recipe> expected = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 45, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 60, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag4"), new("Tag6") }, 2)
            };

        // //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        //filter & obtain query results
        IFilterBy filter = new FilterByTime(75, 130);
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

        //assert
        CollectionAssert.AreEqual(expected, actual);

    }

    //tests for filter by time if time less or equal to 0
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByTimeLessThanZeroTest()
    {
        IFilterBy filter = new FilterByTime(0, 120);
    }

    //tests for filter by time if time is bigger than 420
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByTimeMoreThanFourHoursTest()
    {
        IFilterBy filter = new FilterByTime(50, 421);
    }

    //tests if min time is bigger than max time
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByTimeMinBiggerThanMaxTest()
    {
        IFilterBy filter = new FilterByTime(300, 120);
    }

    //tests for filter by keyword tested name only, description only, both and none
    [TestMethod]
    public void FilterByKeywordTest()
    {
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);


        //creating test recipes
        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        List<MeasuredIngredient> dict = new()
            {
                new( a, 20 ),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new( b, 20 ),
            };
        List<Recipe> recipes = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("recipe Test that is over 10 characters", new User("Bianca", "123456789"), "Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("recipe that is over 10 characters", new User("Bianca", "123456789"), "Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };

        //creating expected results
        List<Recipe> expected = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("recipe Test that is over 10 characters", new User("Bianca", "123456789"), "Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };

        // //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        IFilterBy filter = new FilterByKeyword("Test");
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

        CollectionAssert.AreEqual(expected, actual);
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
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

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
        List<MeasuredIngredient> dict = new()
            {
                new( a, 20 ),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new( b, 20 ),
            };
        recipes.Add(new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2));
        recipes.Add(new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2));

        // creating expected recipes
        List<Recipe> expectedRecipes = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };

        // //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        // calling filtering
        IFilterBy filter = new FilterByIngredients(lookingForIngredient);
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

        CollectionAssert.AreEqual(expectedRecipes, actual);
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
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        //creating test data
        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        List<MeasuredIngredient> dict = new()
            {
                new( a, 20 ),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new( b, 20 ),
            };
        List<Recipe> recipes = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };
        //rate the first recipe 3 star and second 0
        recipes[0].RateRecipe(3,recipes[0].Owner);
        recipes[1].RateRecipe(1,recipes[1].Owner);

        //create expected results
        List<Recipe> expected = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            };

            // //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        IFilterBy filter = new FilterByRating(3);
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

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
    [TestMethod]
    public void FilterByTagsTest()
    {
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        //creating test data
        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        List<MeasuredIngredient> dict = new()
            {
                new( a, 20 ),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new( b, 20 ),
            };
        List<Recipe> recipes = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag2"), new("Tag3") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag4"), new("Tag6") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag4"), new("Tag1") }, 2)
            };

        //create expected data
        List<Recipe> expected = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag2"), new("Tag3") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag4"), new("Tag1") }, 2)
            };

        //filter
        List<Tag> tags = new()
        {
            new("Tag1"),
            new("Tag2")
        };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        IFilterBy filter = new FilterByTags(tags);
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

        CollectionAssert.AreEqual(expected, actual);
    }

    //tests for filter by tags if tags is empty
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FilterByTagsNull()
    {
        IFilterBy filter = new FilterByTags(new List<Tag>());
    }

    //tests for filter by owner
    [TestMethod]
    public void FilterByOwnerTest()
    {
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetUser = new Mock<DbSet<User>>();
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        // creating test data
        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        List<MeasuredIngredient> dict = new()
            {
                new( a, 20 ),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new( b, 20 ),
            };
        List<Recipe> recipes = new()
            {
            new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };

        // creating expected data
        List<Recipe> expected = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
            };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var usersData = recipes.Select(mi => mi.Owner).Distinct().AsQueryable();
        ConfigureDbSetMock<User>(usersData, mockSetUser);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        IFilterBy filter = new FilterByOwner(new User("Bianca", "123456789"));
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

        CollectionAssert.AreEqual(expected, actual);
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
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);
        var mockSetIngredient = new Mock<DbSet<Ingredient>>();
        mockContext.Setup(m => m.RecipeManager_Ingredients).Returns(mockSetIngredient.Object);

        //creating test data
        Ingredient a = new("Apple", Units.Quantity);
        Ingredient b = new("Sugar", Units.Mass);
        List<MeasuredIngredient> dict = new()
            {
                new( a, 20 ),
            };
        List<MeasuredIngredient> dict2 = new()
            {
                new( b, 20 ),
            };
        List<Recipe> recipes = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 2,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 5,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };

        //creating expected data
        List<Recipe> expected = new()
            {
                new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2),
                new("Recipe need 10 characters", new User("Not Bianca", "123456789"), "Test Description", 30, 60, 5,
                new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag1"), new("Tag2") }, 2)
            };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = recipes.AsQueryable();
        ConfigureDbSetMock<Recipe>(data, mockSetRecipe);
        var ingredientsData = recipes.SelectMany(recipe => recipe.Ingredients.Select(mi => mi.Ingredient)).Distinct().AsQueryable();
        ConfigureDbSetMock<Ingredient>(ingredientsData, mockSetIngredient);

        //filter
        IFilterBy filter = new FilterByServings(3, 6);
        IQueryable<Recipe> filteredRecipes = filter.FilterRecipes(data);
        List<Recipe> actual = filteredRecipes.ToList<Recipe>();

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

    //tests for filtering by user
    [TestMethod]
    public void FilterByUsersTest()
    {
        var mockContext = new Mock<RecipesContext>();

        // basic setup
        var mockSetUsers = new Mock<DbSet<User>>();
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUsers.Object);

        // creating new test data
        List<User> users = new()
        {
            new User("Johny", "123456789"),
            new User("Bobbie", "123456789"),
            new User("Doeey", "123456789")
        };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // set up queryable data
        var data = users.AsQueryable();
        ConfigureDbSetMock<User>(data, mockSetUsers);

        // filter the users
        FilterByUsername filter = new FilterByUsername(data);
        User actual = filter.FilterUsers("Johny");

        Assert.AreEqual(new User("Johny", "123456789"), actual);
    }

    //test if given user is null (no test for null return handled by UserController)
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FilterByUsersNullTest()
    {
        var mockContext = new Mock<RecipesContext>();

        List<User> users = new() { };

        //finalize context
        RecipesContext.Instance = mockContext.Object;

        // creating new test data
        var mockSetUsers = new Mock<DbSet<User>>();
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUsers.Object);
        var data = users.AsQueryable();
        ConfigureDbSetMock<User>(data, mockSetUsers);

        // filter the users
        FilterByUsername filter = new FilterByUsername(data);
        User actual = filter.FilterUsers("John");

        Assert.AreEqual(new User("John", "123456789"), actual);
    }
}