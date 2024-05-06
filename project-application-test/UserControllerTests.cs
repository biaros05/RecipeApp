namespace project_application_test;

using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;
using Moq;
using recipes;
using users;

[TestClass]
public class UserControllerTests
{
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
        UserController.Instance = null;
    }

    // test CreateAccount
    [TestMethod]
    public void CreateAccount()
    {
        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;

        var users = new List<User>(){
            new("test user 2", "test blah", "test description")
        }.AsQueryable();

        var expected = new User("test user 1", "test password", "description test");

        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.CreateAccount("test user 1", "test password", "description test");

        mockContext.Verify(mock => mock.Add(It.Is<User>(
            actual => expected.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }
    // test CreateAccount if the User is null/empty
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateAccountIfNull()
    {
        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.CreateAccount(null, "test password", "description test");
    }
    // test CreateAccount if the User already exists (name non-unique)
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateAccountIfUsernameExists()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.CreateAccount("test user 1", "test password", "description test");
    }
    // test authenticate user
    [TestMethod]
    public void AuthenticateUser()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.AuthenticateUser("test user 1", "test password");

        User expected = new User("test user 1", "test password", "test description");

        Assert.AreEqual(expected, UserController.Instance.ActiveUser);
    }
    // test authenticate user if user does not exist
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void AuthenticateUserDoesNotExist()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.AuthenticateUser("dwadadwadwad", "test password");
    }
    // test authenticate user if passwords do not match
    [TestMethod]
    public void AuthenticateUserPasswordWrong()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        bool result = UserController.Instance.AuthenticateUser("test user 1", "dwadawdwa");

        Assert.IsFalse(result);
    }
    // test authenticate user if user is null
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void AuthenticateUserNull()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.AuthenticateUser(null, "test password");
    }
    // test authenticate user if password is null
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AuthenticateUserPasswordNull()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.AuthenticateUser("test user 1", null);
    }
    // test delete accoutn when it matches
    [TestMethod]
    public void DeleteAcount()
    {
        var mockContext = new Mock<RecipesContext>();
        var users = new List<User>(){
            new("test user 1", "test password", "test description"),
            new("test user 2", "test password", "test description")
        }.AsQueryable();

        var expected = new User("test user 1", "test password", "test description");

        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.DeleteAccount("test user 1", "test password");

        mockContext.Verify(mock => mock.Remove(It.Is<User>(
            actual => expected.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    // test delete account if usrname doesnt exist
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteAcountUserNameDoesNotExist()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.DeleteAccount("tedwadawda", "test password");
    }
}