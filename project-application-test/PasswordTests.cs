

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
        public void Pass_Meet_Requirements_Not_Null()
        {
            //Arrange
            string pass="helloWorld";
            //Act
            Password p=new(pass);
            //Assert
            Assert.IsNotNull(p);
        }
    [TestMethod]
        public void Password_Salt_is_Byte_arr_8()
        {
            //Arrange
            string pass="helloWorld";
            //Act
            Password p=new(pass);
            //Assert
            Assert.AreEqual(p.Salt.Length, 8);
        }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
        public void Pass_Doesnt_Meet_Requirements()
        {
            Password p=new Password("hi");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Pass_Doesnt_Meet_Requirements_null_error()
        {
            string value=null;
            Password p=new Password(value);
        }
    // test Password if password does not meet requirements
    // test hashpassword

    //cant test out this method
    // [TestMethod]
    //     public void Pass_checkHash_correct()
    //     {
    //         //Arrange
    //         string pass="helloWorld";
    //         //Act
    //         Password p=new(pass);
    //         //Assert
    //         Assert.IsNotNull(p.Hash);
    //     }
    // [TestMethod]
    //     public void Pass_checkHash_incorrect()
    //     {
    //         //Arrange

    //         //Act

    //         //Assert
    //     }
    
    // test DoPasswordsMatch if passwords match
    [TestMethod]
        public void Passwords_Match()
        {
            //Arrange
            string pass="HelloWorld";
            Password p= new(pass);
            string passwordToCheck="HelloWorld";
            
            //Act
            bool result=p.DoPasswordsMatch(passwordToCheck);
            //Assert
            Assert.IsTrue(result);
        }
    // test if passwords do not match 
    [TestMethod]
        public void Pass_Dont_Match()
        {
            //Arrange
            string pass="HelloWorld";
            Password p= new(pass);
            string passwordToCheck="GoodbyeWorld";
            
            //Act
            bool result=p.DoPasswordsMatch(passwordToCheck);
            //Assert
            Assert.IsFalse(result);
        }
}