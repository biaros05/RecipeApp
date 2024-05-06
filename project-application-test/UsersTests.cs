namespace project_application_test;
using users;
using recipes;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

[TestClass]
public class UsersTests
{
    private static (Mock<RecipesContext>, Mock<DbSet<User>>) GetMocks()
    {
    var mockContext = new Mock<RecipesContext>();
    var mockUsers = new Mock<DbSet<User>>();
    mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUsers.Object);

    return (mockContext, mockUsers);
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

    [TestCleanup()]
    public void Cleanup()
    {
        RecipesContext.Instance = null;
    }

    [TestMethod]
    public void User_Test_Username()
    {
        //Arrange
        string username = "testing";
        string passwrd = "password";
        string description = "description";
        User user1 = new(username,passwrd, description);

        //Act
        //Assert
        Assert.AreEqual("testing", user1.Username);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void User_Test_Username_Too_Short()
    {
        //Arrange
        string username = "abcdefghijklmnopqrstuvwxyz1234567890";
        string passwrd = "password";
        string description = "description";
        

        //Act
        User user1 = new(username,passwrd, description);
        //Assert
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void User_Test_Username_Too_Long()
    {
        //Arrange
        string username = "a";
        string passwrd = "password";
        string description = "description";
        

        //Act
        User user1 = new(username,passwrd, description);
        //Assert
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void User_Test_Username_null()
    {
        //Arrange
        string username = null;
        string passwrd = "password";
        string description = "description";
        

        //Act
        User user1 = new(username,passwrd, description);
        //Assert
    }
    // [TestMethod]
    // public void User_Test_Password()
    // {
    //     //Arrange
    //     string username="testing";
    //     string passwrd="password";
    //     string description="description";
    //     Password pass=new(passwrd);

    //     //Act
    //     User user1=new(username,pass,description);
    //     //Assert
    //     Assert.AreEqual("testing",user1.Username);
    // }
    [TestMethod]
    public void User_Test_Description()
    {
        //Arrange


        string username = "testing";
        string passwrd = "password";
        string description = "description";
        

        //Act
        User user1 = new(username,passwrd, description);
        // List<User> users = new List<User>();
        // users.Add(user1);
        // var userData = users.AsQueryable();

        //Assert
        Assert.AreEqual("description", user1.Description);
    }

    [TestMethod]
    public void User_Test_Description_Null()
    {
        //Arrange
        string username = "testing";
        string passwrd = "password";
        string description = null;
        

        //Act
        User user1 = new(username,passwrd, description);
        //Assert
        Assert.AreEqual(null, user1.Description);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void User_Test_Description_too_long()
    {
        //Arrange
        string username = "testing";
        string passwrd = "password";
        string longDescription = new string('a', 505);
        

        //Act
        User user1 = new(username,passwrd, longDescription);
        //Assert
    }

    

    // [TestMethod]
    // public void User_Tests_UpdateUsername()
    // {
    //     //Arrange
    // var data = new List<User>()
    // {
    //     new User("testing","password","description"),
    //     new User("user2","password2","description2"),
    //     new User( "user3","password3","description3"),
    // }.AsQueryable();


    //     string username = "testing";
    //     string passwrd = "password";
    //     string description = "description";
        
    //     string newUsername = "updatedUser";

    //     var mockContext=new Mock<RecipesContext>();
    //     var mockUser= new Mock<DbSet<User>>();
    //     ConfigureDbSetMock(data,mockUser);
    //     mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
    //     RecipesContext.Instance= mockContext.Object;
    //     var service=RecipesContext.Instance;


    //     //Act
    //     User user1 = new(username,passwrd, description);
    //     user1.UpdateUsername(newUsername);

    //     //Assert
    //     Assert.AreEqual("updatedUser", user1.Username);
    //     mockContext.Verify(mock => mock.SaveChanges(), Times.Once());

    // }

    // [TestMethod]
    // [ExpectedException(typeof(Exception))]
    // public void User_Tests_UpdateUsername_too_Short()
    // {
    //     //Arrange
    //     var data = new List<User>()
    //     {
    //         new("testing","password","description"),
    //         new("user2","password2","description2"),
    //         new( "user3","password3","description3"),
    //     }.AsQueryable();

    //     string username = "testing";
    //     string passwrd = "password";
    //     string description = "description";
        
    //     string newUsername = "a";

    //     var mockContext=new Mock<RecipesContext>();
    //     var mockUser= new Mock<DbSet<User>>();
    //     ConfigureDbSetMock(data,mockUser);
    //     mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
    //     RecipesContext.Instance= mockContext.Object;
    //     var service=RecipesContext.Instance;
    //     //Act
    //     User user1 = new(username,passwrd, description);
    //     user1.UpdateUsername(newUsername);
    //     //Assert

    // }
    // //cant check update password bec Hash is private

    // [TestMethod]
    // public void User_Tests_UpdateFields_Description()
    // {
    //     //Arrange
    //     var data = new List<User>()
    //     {
    //         new("testing","password","description"),
    //         new("user2","password2","description2"),
    //         new( "user3","password3","description3"),
    //     }.AsQueryable();

    //     string username = "testing";
    //     string passwrd = "password";
    //     string description = "description";
        
    //     byte[] byteArray = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    //     string newDescription = "changed";

    //     var mockContext=new Mock<RecipesContext>();
    //     var mockUser= new Mock<DbSet<User>>();
    //     ConfigureDbSetMock(data,mockUser);
    //     mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
    //     RecipesContext.Instance= mockContext.Object;
    //     var service=RecipesContext.Instance;
    //     //Act
    //     User user1 = new(username,passwrd, description);
    //     user1.UpdateFields(newDescription, byteArray);
    //     //Assert
    //     Assert.AreEqual("changed", user1.Description);

    //     mockContext.Verify(mock => mock.SaveChanges(), Times.Once());

    // }
    // [TestMethod]
    // public void User_Tests_UpdateFields_Image()
    // {
    //     //Arrange
    //     var data = new List<User>()
    //     {
    //         new("testing","password","description"),
    //         new("user2","password2","description2"),
    //         new( "user3","password3","description3"),
    //     }.AsQueryable();

    //     string username = "testing";
    //     string passwrd = "password";
    //     string description = "description";
        
    //     byte[] oldImg = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    //     byte[] newImg = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    //     string newDescription = "changed";

    //     var mockContext=new Mock<RecipesContext>();
    //     var mockUser= new Mock<DbSet<User>>();
    //     mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
    //     RecipesContext.Instance= mockContext.Object;
    //     var service=RecipesContext.Instance;
    //     //Act
    //     User user1 = new(username,passwrd, description);
    //     user1.Image = oldImg;
    //     user1.UpdateFields(newDescription, newImg);
    //     //Assert
    //     Assert.AreEqual(newImg, user1.Image);

    //     mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
    // }
    // //coudnt do update picture or remove profile pig bec i dont know howe to test with byte[]

    // [TestMethod]
    // public void User_Tests_RemoveDescription()
    // {
    //     //Arrange
    //     var data = new List<User>()
    //     {
    //         new("testing","password","description"),
    //         new("user2","password2","description2"),
    //         new( "user3","password3","description3"),
    //     }.AsQueryable();

    //     string username = "testing";
    //     string passwrd = "password";
    //     string description = "description";

    //     var mockContext=new Mock<RecipesContext>();
    //     var mockUser= new Mock<DbSet<User>>();
    //     mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
    //     RecipesContext.Instance= mockContext.Object;
    //     var service=RecipesContext.Instance;

    //     //Act
    //     User user1 = new(username,passwrd, description);
    //     user1.RemoveDescription();
    //     //Assert
    //     Assert.IsNull(user1.Description);

    //     mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
    // }
    // [TestMethod]
    // [ExpectedException(typeof(Exception))]
    // public void User_Tests_RemoveDescription_when_null()
    // {
    //     //Arrange
    //     var data = new List<User>()
    //     {
    //         new("testing","password","description"),
    //         new("user2","password2","description2"),
    //         new( "user3","password3","description3"),
    //     }.AsQueryable();

    //     string username = "testing";
    //     string passwrd = "password";
    //     string description = null;
        
    //     var mockContext=new Mock<RecipesContext>();
    //     var mockUser= new Mock<DbSet<User>>();
    //     mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
    //     RecipesContext.Instance= mockContext.Object;
    //     var service=RecipesContext.Instance;
    //     //Act
    //     User user1 = new(username,passwrd, description);
    //     user1.RemoveDescription();
    //     //Assert

    // }

    // test the Equals method

    [TestMethod]
    public void User_Tests_Equals_correct()
    {
        //Arrange

        string username = "testing";
        string passwrd = "password";
        string description = "description";
        
        //Act
        User user1 = new(username,passwrd, description);
        bool result = user1.Username.Equals(username);
        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void User_Tests_Equals_Incorrect()
    {
        //Arrange

        string username = "testing";
        string passwrd = "password";
        string description = "description";
        
        //Act
        User user1 = new(username,passwrd, description);
        bool result = user1.Username.Equals("wrong");
        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void User_Test_AddingToFavorite()
    {
        var mockContext=new Mock<RecipesContext>();
        //Arrange
        var data = new List<User>()
        {
            new("testing","password","description"),
            new("user2","password2","description2"),
            new( "user3","password3","description3"),
        }.AsQueryable();

        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
        {
            recipe
        }.AsQueryable();

        string username = "testing";
        string passwrd = "password";
        string description = "description";
        
        RecipesContext.Instance= mockContext.Object;
        var mockUser= new Mock<DbSet<User>>();
        ConfigureDbSetMock(data,mockUser);
        mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
        var service=RecipesContext.Instance;

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //Act
        User user1 = new(username,passwrd, description);
        user1.AddToFavourites(recipe);
        //Assert
        Assert.IsTrue(user1.UserFavoriteRecipies.Contains(recipe));

        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void User_Test_RemoveFromFavorite()
    {
        var mockContext=new Mock<RecipesContext>();
        //Arrange
        var data = new List<User>()
        {
            new("testing","password","description"),
            new("user2","password2","description2"),
            new( "user3","password3","description3"),
        }.AsQueryable();

        Ingredient i = new("egg", Units.Quantity);
        List<MeasuredIngredient> dict = new();
        dict.Add(new(i, 20));
        Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
            new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        var recipes = new List<Recipe>
            {
                recipe
            }.AsQueryable();

        string username = "testing";
        string passwrd = "password";
        string description = "description";
        
        RecipesContext.Instance= mockContext.Object;
        var mockUser= new Mock<DbSet<User>>();
        ConfigureDbSetMock(data,mockUser);
        mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
        var service=RecipesContext.Instance;

        var mockSetRecipe = new Mock<DbSet<Recipe>>();
        ConfigureDbSetMock(recipes, mockSetRecipe);
        mockContext.Setup(m => m.RecipeManager_Recipes).Returns(mockSetRecipe.Object);

        //Act
        User user1 = new(username,passwrd, description);
        user1.AddToFavourites(recipe);
        // user1.AddToFavourites(recipe2);
        user1.RemoveFromFavourites(recipe);

        //Assert
        Assert.IsFalse(user1.UserFavoriteRecipies.Contains(recipe));

        mockContext.Verify(mock => mock.SaveChanges(), Times.Exactly(2));
    }

    // [TestMethod]
    // public void User_Test_AddingToFavorite()
    // {
    //     //Arrange
    //     Ingredient i = new("egg", Units.Quantity);
    //     List<MeasuredIngredient> dict = new();
    //     dict.Add(new(i, 20));
    //     Recipe recipe = new("Test Recipe", new User("Bianca", "123456789"), "Test Description", 30, 60, 4,
    //         new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<string> { "Tag1", "Tag2" }, 2);

    //     string username="testing";
    //     string passwrd="password";
    //     string description="description";
    //     Password pass=new(passwrd);
    //     //Act
    //     User user1=new(username,pass,description);
    //     user1.AddToFavourites(recipe);
    //     //Assert
    //     Assert.IsTrue(user1.UserFavoriteRecipies.Contains(recipe));
    // }
}