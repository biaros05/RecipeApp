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
    // test CreateAccount
    // test CreateAccount if the User is null/empty
    // test CreateAccount if the User already exists (name non-unique)
    // test authenticate user
    // test authenticate user if user does not exist
    // test authenticate user if passwords do not match
    // test authenticate user if user is null
    // test authenticate user if password is null
    // test delete accoutn when it matches
    // test delete account when passord is incorrect
    // test delete account if usrname doesnt exist
}