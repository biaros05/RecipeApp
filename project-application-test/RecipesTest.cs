namespace project_application_test;

using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.PrepTimeMins = 2400;

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be greater than 4 hours");
        }

        [TestMethod]
        public void PrepTimeMins_SetPrepTimeNegative_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.PrepTimeMins = -100;

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be negative");
        }

        // TESTS FOR COOKTIMEMINS PROPERTY
        [TestMethod]
        public void CookTimeMins_SetCookTimeCorrectly_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.CookTimeMins = 2400;

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be greater than 4 hours");
        }

        [TestMethod]
        public void CookTimeMins_SetCookTimeNegative_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.CookTimeMins = -100;

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "PrepTime cannot be negative");
        }

        // TEST TOTALTIMEMINS PROPERTY
        [TestMethod]
        public void TotalTimeMins_GetTotalTime_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
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
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.RateRecipe(6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be greater than 5");
        }

        // TESTING FOR DIFFICULTY AND RATE DIFFICULTY
        [TestMethod]
        public void RateDifficulty_AddValidRating_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.RateDifficulty(4);

            Assert.AreEqual(4, recipe.DifficultyRating);
        }

        [TestMethod]
        public void RateDifficulty_AddSeveralRatings_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            recipe.RateDifficulty(4);
            recipe.RateDifficulty(10);
            recipe.RateDifficulty(8);
            recipe.RateDifficulty(5);

            Assert.AreEqual(7, recipe.DifficultyRating);
        }

        [TestMethod]
        public void RateDifficulty_RateLessThan1_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.RateDifficulty(-4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Difficulty rating cannot be less than 1");
        }

        [TestMethod]
        public void RateDifficulty_GreaterThan10_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);

            Action act = () => recipe.RateDifficulty(11);

            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Rating cannot be greater than 10");
        }

        // TESTS FOR ADDTAG METHOD
        [TestMethod]
        public void AddTag_CorrectTag_ReturnsTrue()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            List<string> correctTags = new(){"Tag1", "Tag2", "school lunch"};

            recipe.AddTag("school lunch");

            CollectionAssert.AreEqual(correctTags, recipe.Tags);
        }

        [TestMethod]
        public void AddTag_ExistingTag_DoesNotAddTag()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            List<string> correctTags = new(){"Tag1", "Tag2"};

            recipe.AddTag("Tag1");

            CollectionAssert.AreEqual(correctTags, recipe.Tags);
        }

        [TestMethod]
        public void AddTag_EmptyTag_ThrowsArgumentException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            List<string> correctTags = new(){"Tag1", "Tag2"};

            Action act = () => recipe.AddTag("");

            Assert.ThrowsException<ArgumentException>(act, "Cannot add empty tag");
        }

        // TESTING UPDATERECIPE METHOD
        [TestMethod]
        public void UpdateRecipe_CorrectParameters_UpdatesRecipesCorrectly()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            
            string newDescription = "Updated Description";
            int newPrepTime = 45;
            int newCookTime = 75;
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Mass), 300);
            newIngredients.Add(new Ingredient("egg", Units.Quantity), 4);
            List<string> newTags = new List<string> {"NewTag1", "NewTag2"};

            recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);
        
            Assert.AreEqual(newDescription, recipe.Description);
            Assert.AreEqual(newPrepTime, recipe.PrepTimeMins);
            Assert.AreEqual(newCookTime, recipe.CookTimeMins);
            CollectionAssert.AreEquivalent(newIngredients, recipe.Ingredients);
            CollectionAssert.AreEquivalent(newTags, recipe.Tags);
        }

        [TestMethod]
        public void UpdateRecipe_EmptyDescription_UpdatesRecipesCorrectly()
        {
            RecipeController instance = RecipeController.Instance;
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            
            string newDescription = "";
            int newPrepTime = 45;
            int newCookTime = 75;
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Mass), 300);
            List<string> newTags = new List<string> {"NewTag1", "NewTag2"};
            List<Ingredient> ingredients = new(){new("egg", Units.Quantity), new Ingredient("flour", Units.Mass)};

            recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);
        
            Assert.AreEqual("Test Recipe", recipe.Description);
            Assert.AreEqual(newPrepTime, recipe.PrepTimeMins);
            Assert.AreEqual(newCookTime, recipe.CookTimeMins);
            CollectionAssert.AreEquivalent(newIngredients, recipe.Ingredients);
            CollectionAssert.AreEquivalent(newTags, recipe.Tags);
        }

        [TestMethod]
        public void UpdateRecipe_NegativePrepTime_ThrowsException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            
            string newDescription = "Description";
            int newPrepTime = -12;
            int newCookTime = 75;
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"NewTag1", "NewTag2"};

            Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);
        
            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Prep time cannot be negative");
        }

        [TestMethod]
        public void UpdateRecipe_PrepTimeTooLarge_ThrowsException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            
            string newDescription = "Description";
            int newPrepTime = 500;
            int newCookTime = 75;
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"NewTag1", "NewTag2"};

            Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);
        
            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Prep time cannot be greather than 4 hours");
        }

        [TestMethod]
        public void UpdateRecipe_CookTimeTooLarge_ThrowsException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            
            string newDescription = "Description";
            int newPrepTime = 30;
            int newCookTime = 750;
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"NewTag1", "NewTag2"};

            Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);
        
            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Cook time cannot be greater than 4 hours");
        }

        [TestMethod]
        public void UpdateRecipe_CookTimeNegative_ThrowsException()
        {
            Ingredient i = new("egg", Units.Quantity);
            Dictionary<Ingredient, double> dict = new();
            dict.Add(i, 20);
            Recipe recipe = new("Test Recipe", new User("Bianca", "Rossetti"), "Test Description", 30, 60, 4,
                new List<string> { "Step 1", "Step 2" }, dict, new List<string> { "Tag1", "Tag2" }, 2);
            
            string newDescription = "Description";
            int newPrepTime = 30;
            int newCookTime = -10;
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"NewTag1", "NewTag2"};

            Action act = () => recipe.UpdateRecipe(newDescription, newPrepTime, newCookTime, newIngredients, newTags);
        
            Assert.ThrowsException<ArgumentOutOfRangeException>(act, "Cook time cannot be negative");
        }

        // CONSTRUCTOR TESTS:
        [TestMethod]
        public void Constructor_ValidParams_InitializesCorrectly()
        {
            int id = 1;
            string name = "Test Recipe";
            User owner = new User("Bianca", "Rossetti");
            string newDescription = "Test Description";
            int newPrepTime = 30;
            int newCookTime = 60;
            int numServings = 4;
            List<string> instructions = new List<string>{"Step 1", "Step 2"};
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"Tag1", "Tag2"};
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
            int id = 1;
            string name = "Test Recipe";
            User owner = new User("Bianca", "Rossetti");
            string newDescription = "Test Description";
            int newPrepTime = 30;
            int newCookTime = 60;
            int numServings = 4;
            List<string> instructions = new List<string>();
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"Tag1", "Tag2"};
            int budget = 2;

            Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);
            
            Assert.ThrowsException<ArgumentException>(act, "must contain instructions");
        }

        [TestMethod]
        public void Constructor_EmptyIngredients_ThrowsException()
        {
            int id = 1;
            string name = "Test Recipe";
            User owner = new User("Bianca", "Rossetti");
            string newDescription = "Test Description";
            int newPrepTime = 30;
            int newCookTime = 60;
            int numServings = 4;
            List<string> instructions = new List<string>{"Step 1", "Step 2"};
            Dictionary<Ingredient, double> newIngredients = new();
            List<string> newTags = new List<string> {"Tag1", "Tag2"};
            int budget = 2;

            Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);
            
            Assert.ThrowsException<ArgumentException>(act, "must contain ingredients");
        }

        [TestMethod]
        public void Constructor_InvalidBudgetOver3_ThrowsException()
        {
            int id = 1;
            string name = "Test Recipe";
            User owner = new User("Bianca", "Rossetti");
            string newDescription = "Test Description";
            int newPrepTime = 30;
            int newCookTime = 60;
            int numServings = 4;
            List<string> instructions = new List<string>{"Step 1", "Step 2"};
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"Tag1", "Tag2"};
            int budget = 4;

            Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);
            
            Assert.ThrowsException<ArgumentException>(act, "budget cannot be greater than 3");
        }

        [TestMethod]
        public void Constructor_InvalidBudgetLessThan1_ThrowsException()
        {
            int id = 1;
            string name = "Test Recipe";
            User owner = new User("Bianca", "Rossetti");
            string newDescription = "Test Description";
            int newPrepTime = 30;
            int newCookTime = 60;
            int numServings = 4;
            List<string> instructions = new List<string>{"Step 1", "Step 2"};
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"Tag1", "Tag2"};
            int budget = 0;

            Action act = () => new Recipe(name, owner, newDescription, newPrepTime, newCookTime, numServings, instructions, newIngredients, newTags, budget);
            
            Assert.ThrowsException<ArgumentException>(act, "budget cannot be greater than 3");
        }

        // COPY CONSTRUCTOR TESTS
        [TestMethod]
        public void CopyConstructor_ValidParams_InitializesCorrectly()
        {
            int id = 1;
            string name = "Test Recipe";
            User owner = new User("Bianca", "Rossetti");
            string newDescription = "Test Description";
            int newPrepTime = 30;
            int newCookTime = 60;
            int numServings = 4;
            List<string> instructions = new List<string>{"Step 1", "Step 2"};
            Dictionary<Ingredient, double> newIngredients = new();
            newIngredients.Add(new Ingredient("flour", Units.Quantity), 300);
            List<string> newTags = new List<string> {"Tag1", "Tag2"};
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