namespace project_application_test;
using users;
using recipes;

[TestClass]
public class UsersTests
{
    // ANTHONY -------------------------------
    // test the creation of EVERY FIELD.
    [TestMethod]
        public void User_Test_Username()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,description);
            //Assert
            Assert.AreEqual("testing",user1.Username);
        }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
        public void User_Test_Username_Too_Short()
        {
            //Arrange
            string username="abcdefghijklmnopqrstuvwxyz1234567890";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,description);
            //Assert
        }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
        public void User_Test_Username_Too_Long()
        {
            //Arrange
            string username="a";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,description);
            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]  
        public void User_Test_Username_null()
        {
            //Arrange
            string username=null;
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,description);
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
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,description);
            //Assert
            Assert.AreEqual("description",user1.Description);
        }

        [TestMethod]
        public void User_Test_Description_Null()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description=null;
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,description);
            //Assert
            Assert.AreEqual(null,user1.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void User_Test_Description_too_long()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string longDescription = new string('a', 505);
            Password pass=new(passwrd);
            
            //Act
            User user1=new(username,pass,longDescription);
            //Assert
        }

        [TestMethod]
        public void User_Tests_UpdateUsername()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            string newUsername="updatedUser";
            //Act
            User user1=new(username,pass,description);
            user1.UpdateUsername(newUsername);
            //Assert
            Assert.AreEqual("updatedUser",user1.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void User_Tests_UpdateUsername_too_Short()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            string newUsername="a";
            //Act
            User user1=new(username,pass,description);
            user1.UpdateUsername(newUsername);
            //Assert
            
        }
        //cant check update password bec Hash is private

        [TestMethod]
        public void User_Tests_UpdateFields_Description()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            byte[] byteArray = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            string newDescription="changed";
            //Act
            User user1=new(username,pass,description);
            user1.UpdateFields(newDescription,byteArray);
            //Assert
            Assert.AreEqual("changed",user1.Description);
        }

        public void User_Tests_UpdateFields_Image()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            byte[] oldImg = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            byte[] newImg = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            string newDescription="changed";
            //Act
            User user1=new(username,pass,description);
            user1.Image=oldImg;
            user1.UpdateFields(newDescription,newImg);
            //Assert
            Assert.AreEqual(newImg,user1.Image);
        }
        //coudnt do update picture or remove profile pig bec i dont know howe to test with byte[]

        [TestMethod]
        public void User_Tests_RemoveDescription()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            //Act
            User user1=new(username,pass,description);
            user1.RemoveDescription();
            //Assert
            Assert.IsNull(user1.Description);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void User_Tests_RemoveDescription_when_null()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description=null;
            Password pass=new(passwrd);
            //Act
            User user1=new(username,pass,description);
            user1.RemoveDescription();
            //Assert
            
        }

        // test the Equals method

        [TestMethod]
        public void User_Tests_Equals_correct()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            //Act
            User user1=new(username,pass,description);
            bool result=user1.Username.Equals("testing");
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void User_Tests_Equals_Incorrect()
        {
            //Arrange
            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            //Act
            User user1=new(username,pass,description);
            bool result=user1.Username.Equals("wrong");
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void User_Test_AddingToFavorite()
        {
            //Arrange
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", new Password("123456789")), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            //Act
            User user1=new(username,pass,description);
            user1.AddToFavourites(recipe);
            //Assert
            Assert.IsTrue(user1.UserFavoriteRecipies.Contains(recipe));
        }

        [TestMethod]
        public void User_Test_RemoveFromFavorite()
        {
            //Arrange
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", new Password("123456789")), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            // Recipe recipe2 = new("Test Recipe2", new User("Bianca", new Password("123456789")), "Test Description", 30, 60, 4,
            //     new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            string username="testing";
            string passwrd="password";
            string description="description";
            Password pass=new(passwrd);
            //Act
            User user1=new(username,pass,description);
            // user1.AddToFavourites(recipe);
            // user1.AddToFavourites(recipe2);
            user1.RemoveFromFavourites(recipe);

            //Assert
            Assert.IsFalse(user1.UserFavoriteRecipies.Contains(recipe));
        }

        // [TestMethod]
        // public void User_Test_AddingToFavorite()
        // {
        //     //Arrange
        //     Ingredient i = new("egg", Units.Quantity);
        //     Dictionary<Ingredient, double> dict = new();
        //     dict.Add(i, 20);
        //     Recipe recipe = new("Test Recipe", new User("Bianca", new Password("123456789")), "Test Description", 30, 60, 4,
        //         new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

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
    // CHEN --------------------------------------
    // test the creation of EVERY FIELD.
    // test UpdatePassword
    // test UpdatePassword if password null
    // test UpdatePassword if password empty
    // test UpdatePassword if password does not meet requirements
    // test AddToFavourites
    // test AddToFavourites if recipe does not exist
    // test AddToFavourites if recipe is null
    // test RemoveFromFavourites
    // test RemoveFromFavourites if recipe does not exist
    // test RemoveFromFavourites if recipe is null

    
    
    // test RemoveProfilePicture
    // test RemoveProfilePicture if Image already empty
    // test RemoveDescription
    // test RemoveDescription if description already empty
    

}