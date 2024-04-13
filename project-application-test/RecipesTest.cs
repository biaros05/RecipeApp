namespace project_application_test;
using recipes;
using users;

[TestClass]
public class RecipesTest
{
    // TESTS FOR NAME PROPERTY
    [TestMethod]
        public void Name_SetValidName_SetsNameCorrectly()
        {
            // Arrange
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
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
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.Name = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(act, "Null name should throw exception.");
        }

        [TestMethod]
        public void Name_SetShortName_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
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
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
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
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            string validDescription = "This is a valid description";

            recipe.Description = validDescription;

            Assert.AreEqual(validDescription, recipe.Description);
        }

        [TestMethod]
        public void Description_SetEmptyDescription_SetsEmptyDescription_SetsDescriptionAsRecipeName()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            string EmptyDescription = "";

            recipe.Description = EmptyDescription;

            Assert.AreEqual("Test Recipe", recipe.Description);
        }

        [TestMethod]
        public void Description_SetNullDescription_SetsDescriptionAsRecipeName()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.Description = null;

            Assert.AreEqual("Test Recipe", recipe.Description);
        }

        // TESTS FOR PREPTIMEMINS PROPERTY
        [TestMethod]
        public void PrepTimeMins_SetPrepTimeCorrectly_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.PrepTimeMins = 30;

            Assert.AreEqual(30, recipe.PrepTimeMins);
        }

        [TestMethod]
        public void PrepTimeMins_SetPrepTimeGreaterThan4_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.PrepTimeMins = 2400;

            Assert.ThrowsException<ArgumentException>(act, "PrepTime cannot be greater than 4 hours");
        }

        [TestMethod]
        public void PrepTimeMins_SetPrepTimeNegative_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.PrepTimeMins = -100;

            Assert.ThrowsException<ArgumentException>(act, "PrepTime cannot be negative");
        }

        // TESTS FOR COOKTIMEMINS PROPERTY
        [TestMethod]
        public void CookTimeMins_SetCookTimeCorrectly_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.CookTimeMins = 30;

            Assert.AreEqual(30, recipe.CookTimeMins);
        }

        [TestMethod]
        public void CookTimeMins_SetCookTimeGreaterThan4_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.CookTimeMins = 2400;

            Assert.ThrowsException<ArgumentException>(act, "PrepTime cannot be greater than 4 hours");
        }

        [TestMethod]
        public void CookTimeMins_SetCookTimeNegative_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.CookTimeMins = -100;

            Assert.ThrowsException<ArgumentException>(act, "PrepTime cannot be negative");
        }

        // TEST TOTALTIMEMINS PROPERTY
        [TestMethod]
        public void TotalTimeMins_GetTotalTime_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            double totalTime = recipe.TotalTimeMins;

            Assert.AreEqual(1.5, totalTime);
        }
        
        // TEST FOR RATING & RATERECIPE
        [TestMethod]
        public void RateRecipe_AddValidRating_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.RateRecipe(4);

            Assert.AreEqual(4, recipe.Rating);
        }

        [TestMethod]
        public void RateRecipe_AddSeveralRatings_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.RateRecipe(4);
            recipe.RateRecipe(5);
            recipe.RateRecipe(5);
            recipe.RateRecipe(3);

            Assert.AreEqual(4.25, recipe.Rating);
        }

        [TestMethod]
        public void RateRecipe_RateLessThan1_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.RateRecipe(-4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be less than 1");
        }

        [TestMethod]
        public void RateRecipe_GreaterThan5_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new(1, "Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.RateRecipe(6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be less than 1");
        }

    // test delete recipe
    // test delete recipe if recipe does not exits
    // test delete recipe if the user is not the owner
    // test create recipe
    // test create recipe if recipe already exists
    // test update description 
    // test update preptimeMins
    // test update preptimeMins if the time is less than the requirement
    // tes update cooktimeMins
    // tes update cooktimeMins if the time is less than the requirement
    // test update servings
    // test update servings if servings negative
    // test update ingredients
    // test update ingredients if ingredient doesnt exist (?) --> need to add it first?
    // test update tags
    // test update description if null
    // test update preptimeMins if null
    // tes update cooktimeMins if null
    // test update servings if null
    // test update ingredients if null
    // test update tags if null
    // test update rating if null
    // test UpdateDescription if the user isnt the owner
    // test RateRecipe if the rating is less than 0
    // test RateRecipe if the rating is more than 5
    // test RateRecipe if the rating is null
    // test RateRecipe
    // test UpdateDescription 
    // test UpdateDescription --> one new test for every field that could be invalid
    // this method will take into account the ingredients and calculate the budget for a given recipe
    // test GetRecipeBudgetX
    // test GetRecipeBudget if there are no ingredients(?)
    // test RateDifficulty
    // test RateDifficulty if difficulty does not meet requirements (<0 or >3)
    
}