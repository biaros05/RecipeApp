

using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using System.Collections.ObjectModel;
using users;

namespace project_application_test;

[TestClass]
public class PasswordTests
{
    [TestMethod]
        public void Password_Salt_is_Byte_arr_8()
        {
            //Arrange
            //Act
            byte[] salt=Password.GenerateSalt();
            //Assert
            Assert.AreEqual(salt.Length, 8);
        }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
        public void Pass_Doesnt_Meet_Requirements()
        {
            byte[] salt=Password.GenerateSalt();
            Password.HashPassword(salt,"hi");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Pass_Doesnt_Meet_Requirements_null_error()
        {
            string pass=null;
            byte[] salt=Password.GenerateSalt();
            string hash=Password.HashPassword(salt,pass);
        }

    // test DoPasswordsMatch if passwords match
    [TestMethod]
        public void Passwords_Match()
        {
            //Arrange
            string pass="hellooo";
            byte[] salt=Password.GenerateSalt();
            string hash=Password.HashPassword(salt,pass);
            
            //Act
            bool result=Password.DoPasswordsMatch("hellooo",salt,hash);
            //Assert
            Assert.IsTrue(result);
        }
    // test if passwords do not match 
    [TestMethod]
        public void Pass_Dont_Match()
        {
            //Arrange
            string pass="hellooo";
            byte[] salt=Password.GenerateSalt();
            string hash=Password.HashPassword(salt,pass);
            
            //Act
            bool result=Password.DoPasswordsMatch("goodbye",salt,hash);
            //Assert
            Assert.IsFalse(result);
        }
}