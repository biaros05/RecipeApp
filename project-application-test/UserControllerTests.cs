namespace project_application_test;

using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;
using Moq;
using users;

[TestClass]
public class UserControllerTests
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
    
    [TestMethod]
    public void UserController_UpdateUser()
    {
        //Arrange
    var data = new List<User>()
    {
        new User("testing","password","description"),
        new User("user2","password2","description2"),
        new User( "user3","password3","description3"),
    }.AsQueryable();


        string username = "testing";
        string passwrd = "password";
        string description = "description";
        
        string newUsername = "updatedUser";
        string newDescription="some sort of new description";
        byte[] byteArray = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
        string newPass="newPassword";

        var mockContext=new Mock<RecipesContext>();
        var mockUser= new Mock<DbSet<User>>();
        ConfigureDbSetMock(data,mockUser);
        mockContext.Setup(mock => mock.RecipeManager_Users).Returns(mockUser.Object);
        RecipesContext.Instance= mockContext.Object;
        var service=RecipesContext.Instance;

        //Act
        User user1 = new(username,passwrd, description);
        UserController.Instance.ActiveUser=user1;
        UserController.Instance.UpdateUser(newUsername,newDescription,byteArray,newPass);

        //Assert
        Assert.AreEqual("updatedUser", user1.Username);
        mockContext.Verify(mock => mock.Update(It.IsAny<User>()), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once());

    }


    [TestCleanup()]
    public void Cleanup()
    {
        RecipesContext.Instance = null;
    }

    // test CreateAccount
    [TestMethod]
    public void CreateAccount()
    {
        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.CreateAccount("test user 1", "test password", "description test");

        var expected = new User("test user 1", "test password", "description test");

        mockSetUser.Verify(mock => mock.Add(It.Is<User>(
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
    [ExpectedException(typeof(Exception))]
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

        UserController.Instance.AuthenticateUser("test user 1", "dwadawdwa");
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
    [ExpectedException(typeof(Exception))]
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
        var users = new List<User>(){
            new("test user 1", "test password", "test description"),
            new("test user 2", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.DeleteAccount("test user 1", "test password");

        var expected = new User("test user 1", "test password", "test description");

        mockSetUser.Verify(mock => mock.Remove(It.Is<User>(
            actual => expected.Equals(actual))), Times.Once);
        mockContext.Verify(mock => mock.SaveChanges(), Times.Once);
    }
    // test delete account when passord is incorrect
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteAcountWrongPassword()
    {
        var users = new List<User>(){
            new("test user 1", "test password", "test description")
        }.AsQueryable();

        var mockContext = new Mock<RecipesContext>();
        RecipesContext.Instance = mockContext.Object;
        var mockSetUser = new Mock<DbSet<User>>();
        ConfigureDbSetMock(users, mockSetUser);
        mockContext.Setup(m => m.RecipeManager_Users).Returns(mockSetUser.Object);

        UserController.Instance.DeleteAccount("test user 1", "tadwadaw");
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