using System.Data;
using users;
using recipes;
using System.Globalization;
using filtering;
using System.Xml;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
namespace Ui;

public class Program
{
    private static MainMenuOption[] MainMenuOptions { get; } =
    Enum.GetValues<MainMenuOption>();

    private static UserOptions[] UserOption { get; } =
    Enum.GetValues<UserOptions>();

    private static Units[] UnitsOptions { get; } =
    Enum.GetValues<Units>();
    private static FilterRecipeSearch[] FilterRecipeSearches { get; } =
    Enum.GetValues<FilterRecipeSearch>();

    private static int ValidateInt()
    {
        int num = 0;
        bool validInput = false;
        while (!validInput && num >= 0)
        {
            Console.WriteLine("Enter an integer number:");
            string? input = Console.ReadLine();
            validInput = int.TryParse(input, out num);
        }
        return num;
    }

    private static double ValidateDouble()
    {
        double num = 0;
        bool validInput = false;
        while (!validInput && num >= 0)
        {
            Console.WriteLine("Enter a decimal number:");
            string? input = Console.ReadLine();
            validInput = double.TryParse(input, out num);
        }
        return num;
    }

    private static List<Instruction> FillInstructions()
    {
        Console.WriteLine("Type 'done' when you are finished");
        List<Instruction> instructions = new();
        string input = Console.ReadLine();
        int stepNum = 1;

        while (!input.Equals("done"))
        {
            instructions.Add(new Instruction(stepNum, input));
            stepNum++;
            input = Console.ReadLine();
        }
        return instructions;
    }

    private static Units ChooseUnit()
    {
        Units? chosenOption = null;
        while (true)
        {
            chosenOption = ConsoleUtils.GetUserChoice(
                "Choose the next operation (Volume will default to ml, Mass will default to g)", UnitsOptions);

            if (!chosenOption.HasValue)
            {
                continue;
            }

            switch (chosenOption)
            {
                case Units.Mass:
                    chosenOption = Units.Mass;
                    break;
                case Units.Volume:
                    chosenOption = Units.Volume;
                    break;
                case Units.Quantity:
                    chosenOption = Units.Quantity;
                    break;
            }
            return (Units)chosenOption;
        }
    }

    private static Ingredient NewIngredient()
    {
        bool successful = false;
        Ingredient? i = null;
        while (!successful)
        {
            try
            {
                Console.WriteLine("Ingredient name:");
                string name = Console.ReadLine();
                Units unit = ChooseUnit();
                i = new Ingredient(name, unit);
                successful = true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return (Ingredient)i!;
    }

    private static List<MeasuredIngredient> FillIngredients()
    {
        Console.WriteLine("What ingredients would you like to add?");
        List<MeasuredIngredient> ingredients = new();
        string input = "";

        while (!input.Equals("done"))
        {
            Ingredient i = NewIngredient();
            Console.WriteLine("Quantity:");
            double quantity = ValidateDouble();
            ingredients.Add(new(i, quantity));
            Console.WriteLine("Type 'done' if you are finished");
            input = Console.ReadLine();
        }
        return ingredients;
    }

    // idk if this needs any more implementation
    private static void LoginOrRegister()
    {
        while (UserController.Instance.ActiveUser == null)
        {
            UserOptions? chosenOption = ConsoleUtils.GetUserChoice(
                "Choose the next operation", UserOption);

            if (!chosenOption.HasValue)
            {
                break;
            }

            switch (chosenOption)
            {
                case UserOptions.Login:
                    Login();
                    break;
                case UserOptions.Register:
                    Register();
                    break;
            }
        }
    }

    private static void Login()
    {
        Console.WriteLine("Enter your username");
        string username = Console.ReadLine();
        Console.WriteLine("Enter your password");
        string password = Console.ReadLine();
        if (UserController.Instance.AuthenticateUser(username, password))
        {
            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Username or password wrong");
        }
        ConsoleUtils.WaitUserPressKey();
    }

    private static void Register()
    {
        Console.WriteLine("Enter your username");
        string username = Console.ReadLine();
        Console.WriteLine("Enter your password");
        string password = Console.ReadLine();
        Console.WriteLine("Enter a description for your user");
        string description = Console.ReadLine();
        Console.WriteLine("Registration successful!");
        UserController.Instance.CreateAccount(username, password, description);
    }

    private static List<Tag> FillTags()
    {
        Console.WriteLine("What Tags would you like to add?");
        List<Tag> tags = new();
        string tag = "";

        while (!tag.Equals("done"))
        {
            Console.WriteLine("Enter your tag, type 'done' if you are finished:");
            tag = Console.ReadLine();
            tags.Add(new Tag(tag));
        }
        return tags;
    }

    private static void CreateRecipe()
    {
        Console.WriteLine("Name of Recipe:");
        string name = Console.ReadLine();
        User owner = UserController.Instance.ActiveUser;
        Console.WriteLine("Description of Recipe:");
        string description = Console.ReadLine();
        Console.WriteLine("Prep time of Recipe (in minutes):");
        int prepTimeMins = ValidateInt();
        Console.WriteLine("Cook time of Recipe (in minutes):");
        int cookTimeMins = ValidateInt();
        Console.WriteLine("Number of servings:");
        int numberOfServings = ValidateInt();
        Console.WriteLine("List the ingredients:");
        List<MeasuredIngredient> ingredients = FillIngredients();
        Console.WriteLine("List the instructions:");
        List<Instruction> instructions = FillInstructions();
        Console.WriteLine("Add Tags to recipe:");
        List<Tag> tags = FillTags();
        Console.WriteLine("Budget on a scale of 1-3:");
        int budget = ValidateInt();
        Recipe newRecipe = new Recipe(name, owner, description, prepTimeMins, cookTimeMins, numberOfServings, instructions, ingredients, tags, budget);

        RecipeController.CreateRecipe(newRecipe);
    }

    private static void EditRecipe()
    {
        PrintRecipes();
        Console.WriteLine("Enter the recipe you would like to edit");
        int num = ValidateInt();
        Recipe toEdit = ReturnOneRecipe(num);
        Console.WriteLine("Enter the new description");
        string description = Console.ReadLine();
        Console.WriteLine("Prep time of Recipe (in minutes):");
        int prepTimeMins = ValidateInt();
        Console.WriteLine("Cook time of Recipe (in minutes):");
        int cookTimeMins = ValidateInt();
        Console.WriteLine("List the ingredients:");
        List<MeasuredIngredient> ingredients = FillIngredients();
        Console.WriteLine("Add Tags to recipe:");
        List<Tag> tags = FillTags();

        try
        {
            toEdit.UpdateRecipe(description, prepTimeMins, cookTimeMins, ingredients, tags);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
        ConsoleUtils.WaitUserPressKey();

    }

    private static void PrintRecipes()
    {
        var context = RecipesContext.Instance;
        List<Recipe> retrieveRecipes = context.RecipeManager_Recipes
            .Include(recipe => recipe.Tags)
            .Include(recipe => recipe._ratings)
            .Include(recipe => recipe._difficulties)
            .Include(recipe => recipe.Owner)
            .Include(recipe => recipe.Ingredients)
                .ThenInclude(measuredIngredient => measuredIngredient.Ingredient)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.UserFavourite)
            .ToList();
        int num = 1;
        foreach (Recipe r in retrieveRecipes)
        {
            Console.WriteLine($"{num}. {r}");
            num++;
        }
        ConsoleUtils.WaitUserPressKey();
    }

    private static void PrintUsers()
    {
        var context = RecipesContext.Instance;
        List<User> retrieveUsers = context.RecipeManager_Users.ToList<User>();
        int num = 1;
        foreach (User u in retrieveUsers)
        {
            Console.WriteLine($"{num}. {u}");
            num++;
        }
        ConsoleUtils.WaitUserPressKey();
    }

    /// <summary>
    /// prints the type of filters
    /// </summary>
    private static void PrintFilters()
    {
        Console.Write("Current Filters: ");
        foreach (IFilterBy filter in RecipeController.Instance.Filters)
        {
            Console.Write($"{filter}, ");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Gets a list of ingredient names which is turned into ingredient objects from user input
    /// can be one or many
    /// </summary>
    /// <returns>a list of ingredient objects</returns>
    private static List<Ingredient> GetIngredients()
    {
        List<Ingredient> ingredients = new();
        bool keepAddingIngredients = true;
        while (keepAddingIngredients)
        {
            try
            {
                Console.WriteLine("Enter an ingredient name, Enter 'done' to continue");
                string? ingredientName = Console.ReadLine() ?? throw new Exception("ingredientName cannot be null");
                if (ingredientName.Equals("done"))
                {
                    keepAddingIngredients = false;
                }
                else
                {
                    ingredients.Add(new Ingredient(ingredientName, Units.Volume)); // hardcoded Unit because not necesssary for comparison
                    Console.Write("added ingredient to list: ");
                    foreach (Ingredient ingredient in ingredients)
                    {
                        Console.Write(ingredient.Name + ", ");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a string");
            }
        }
        return ingredients;
    }

    /// <summary>
    /// Gets a keyword from user input
    /// </summary>
    /// <returns>a string keyword</returns>
    private static string GetKeyword()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter a keyword:");
                string? keyword = Console.ReadLine();
                if (keyword == null || keyword == "")
                {
                    throw new Exception("Keyword cannot be null or empty");
                }
                Console.WriteLine("Filtering by " + keyword + " will be added. Press Enter to continue");
                Console.ReadLine();
                return keyword;
            }
            catch (Exception)
            {
                Console.WriteLine("Keyword cannot be null or empty");
            }
        }
    }

    /// <summary>
    /// Gets a username from user input and turns it into a user object
    /// </summary>
    /// <returns>a user object with given username from user input</returns>
    private static User GetUser()
    {
        while (true)
        {
            try
            {
                PrintUsers();
                Console.WriteLine("Enter a username who's recipes you would like to search for:");
                string? username = Console.ReadLine();
                if (username == null || username == "")
                {
                    throw new Exception("username cannot be null or empty");
                }
                Console.WriteLine("Filtering by user " + username + " will be added. Press Enter to continue");
                Console.ReadLine();
                IQueryable<User> userQuery = RecipesContext.Instance.RecipeManager_Users;
                FilterByUsername filter = new FilterByUsername(userQuery);
                User toSearch = filter.FilterUsers(username);
                return toSearch;
            }
            catch (Exception)
            {
                Console.WriteLine("The entered username cannot be null or empty and has to be within 5 and 50 characters");
            }
        }
    }

    /// <summary>
    /// gets a interger representing a rating
    /// </summary>
    /// <returns>returns </returns>
    private static int GetRating()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter a rating");
                int rating = ValidateInt();
                if (rating > 5 || rating < 1)
                {
                    throw new Exception("rating must be out of 5 stars");
                }
                return rating;
            }
            catch (Exception)
            {
                Console.WriteLine("The rating must be out of 5 stars");
            }
        }
    }

    /// <summary>
    /// Asks the user to enter a minimum serving size and max serving size
    /// </summary>
    /// <returns>FilterByServings object</returns>
    private static FilterByServings GetServing()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter a minimum serving size");
                int servingMin = ValidateInt();
                Console.WriteLine("Enter a maximum serving size");
                int servingMax = ValidateInt();
                return new FilterByServings(servingMin, servingMax);
            }
            catch (Exception)
            {
                Console.WriteLine("min serving size must be smaller than max serving size");
            }
        }
    }

    /// <summary>
    /// asks the user to enter tags to filter
    /// </summary>
    /// <returns>returns a list of string</returns>
    private static List<Tag> GetTags()
    {
        List<Tag> tags = new();
        bool keepAddingTags = true;
        while (keepAddingTags)
        {
            try
            {
                Console.WriteLine("Enter a tag, Enter 'done' to continue");
                string? tag = Console.ReadLine() ?? throw new Exception("tag cannot be null");
                if (tag.Equals("done"))
                {
                    keepAddingTags = false;
                }
                else
                {
                    tags.Add(new Tag(tag));
                    Console.Write("added ingredient to list: ");
                    foreach (Tag t in tags)
                    {
                        Console.Write(t.TagName + ", ");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a string");
            }
        }
        return tags;
    }

    /// <summary>
    /// asks the user to create a filter by time object that takes min and max
    /// </summary>
    /// <returns>a FilterByTime object</returns>
    private static FilterByTime GetTime()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter a minimum time");
                int timeMin = ValidateInt();
                Console.WriteLine("Enter a maximum time (420 minutes MAX)");
                int timeMax = ValidateInt();
                return new FilterByTime(timeMin, timeMax);
            }
            catch (Exception)
            {
                Console.WriteLine("min time must be greater than max time and must be 420 minutes MAX");
            }
        }
    }

    private static void PrintFilteredList(List<Recipe> recipes)
    {
        Console.Write("Recipes: ");
        foreach (Recipe recipe in recipes)
        {

            Console.WriteLine(recipe);
        }
    }

    private static void FilterRecipeSearch()
    {
        while (true)
        {
            FilterRecipeSearch? chosenOption = ConsoleUtils.GetUserChoice("Choose to filter", FilterRecipeSearches);

            if (!chosenOption.HasValue)
            {
                break;
            }

            switch (chosenOption)
            {
                case Ui.FilterRecipeSearch.FilterByIngredient:
                    List<Ingredient> ingredients = GetIngredients();
                    RecipeController.Instance.AddFilter(new FilterByIngredients(ingredients));
                    break;
                case Ui.FilterRecipeSearch.FilterByKeyword:
                    string keyword = GetKeyword();
                    RecipeController.Instance.AddFilter(new FilterByKeyword(keyword));
                    break;
                case Ui.FilterRecipeSearch.FilterByOwner:
                    User user = GetUser();
                    RecipeController.Instance.AddFilter(new FilterByOwner(user));
                    break;
                case Ui.FilterRecipeSearch.FilterByRating:
                    int rating = GetRating();
                    RecipeController.Instance.AddFilter(new FilterByRating(rating));
                    break;
                case Ui.FilterRecipeSearch.FilterByServing:
                    RecipeController.Instance.AddFilter(GetServing());
                    break;
                case Ui.FilterRecipeSearch.FilterByTag:
                    List<Tag> tags = GetTags();
                    RecipeController.Instance.AddFilter(new FilterByTags(tags));
                    break;
                case Ui.FilterRecipeSearch.FilterByTime:
                    RecipeController.Instance.AddFilter(GetTime());
                    break;
                case Ui.FilterRecipeSearch.CompletedFilter:
                    Console.WriteLine("Filtering... Press 'Enter' to continue");
                    PrintFilteredList(RecipeController.Instance.FilterBy());
                    Console.ReadLine();
                    break;
                case Ui.FilterRecipeSearch.RemoveAllFilters:
                    Console.WriteLine("Removing filters... Press 'Enter' to continue");
                    RecipeController.Instance.RemoveAllFilters();
                    Console.ReadLine();
                    break;
                case Ui.FilterRecipeSearch.ShowFilters:
                    PrintFilters();
                    Console.WriteLine("Press 'Enter' to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RateRecipe()
    {
        PrintRecipes();
        Console.WriteLine("which recipie would you like to rate");
        int num = Convert.ToInt32(Console.ReadLine());
        Recipe recipe = ReturnOneRecipe(num);
        Console.WriteLine("what would you like to rate this recipe out of 5?");
        int rating = Convert.ToInt32(Console.ReadLine());
        recipe.RateRecipe(rating, UserController.Instance.ActiveUser);
        // Console.WriteLine(recipe.Rating);
        Console.WriteLine("the recipe rating has been updated");
        // Console.WriteLine(recipe);

        ConsoleUtils.WaitUserPressKey();
    }

    private static void AddRecipeToFavourites()
    {
        PrintRecipes();
        Console.WriteLine("which recipie would you like to add to your favorite list");
        int num = Convert.ToInt32(Console.ReadLine());

        Recipe recipe = ReturnOneRecipe(num);

        UserController.Instance.ActiveUser.AddToFavourites(recipe);

        Console.WriteLine("recipe has been added to your favorites");
        Console.WriteLine(recipe);

        ConsoleUtils.WaitUserPressKey();
    }

    //return the correct object from the recipe list
    public static Recipe ReturnOneRecipe(int num)
    {
        var context = RecipesContext.Instance;
        
        List<Recipe> retrieveRecipes = context.RecipeManager_Recipes
            .Include(recipe => recipe.Tags)
            .Include(recipe => recipe._ratings)
            .Include(recipe => recipe._difficulties)
            .Include(recipe => recipe.Owner)
            .Include(recipe => recipe.Ingredients)
                .ThenInclude(measuredIngredient => measuredIngredient.Ingredient)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.UserFavourite)
            .ToList();
        return retrieveRecipes[num - 1];
    }
    //return the correct object from the user favorite recipe list
    public static Recipe ReturnOneRecipeFromFave(int num)
    {
        int count = 0;
        return UserController.Instance.ActiveUser.UserFavoriteRecipies.ToList()[num - 1];
    }

    private static void RemoveRecipeFromFavourites()
    {
        PrintFavoriteList();
        Console.WriteLine("which recipie would you like to add to your favorite list");
        int num = System.Convert.ToInt32(Console.ReadLine());

        Recipe recipe = ReturnOneRecipeFromFave(num);

        UserController.Instance.ActiveUser.RemoveFromFavourites(recipe);

        Console.WriteLine("recipe has been removed from your favorites");
        Console.WriteLine("updated look of favorite list");
        PrintFavoriteList();
    }

    //show the active user their favorited recipes 
    private static void PrintFavoriteList()
    {
        int num = 1;
        foreach (Recipe r in UserController.Instance.ActiveUser.UserFavoriteRecipies)
        {
            Console.WriteLine($"{num}. {r}");
            num++;
        }
        ConsoleUtils.WaitUserPressKey();
    }

    public static void Main()
    {
        LoginOrRegister();


        // Ingredient i = new("egg", Units.Quantity);
        // Ingredient b = new("meat", Units.Mass);
        // Ingredient c = new("food", Units.Mass);
        // List<MeasuredIngredient> dict = new()
        // {
        //     new(b, 30 )
        // };
        // Recipe recipe = new("Test Recipe", UserController.Instance.ActiveUser, "Test Description", 120, 60, 10,
        //     new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict, new List<Tag> { new("Tag1"), new("Tag2") }, 2);
        // RecipeController.Instance.CreateRecipe(recipe);

        // List<MeasuredIngredient> dict2 = new()
        // {
        //     new( i, 20 ),
        //     new( c, 10 )
        // };
        // List<MeasuredIngredient> dict3 = new()
        // {
        //     new( c, 10 ),
        //     new( i, 20 ),
        //     new( b, 30 )
        // };


        // Recipe recipe2 = new("TEST RECIPE2 meat", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
        //     new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict2, new List<Tag> { new("Tag3"), new("Tag2") }, 2);
        // Recipe recipe3 = new("GRRRRRRR meat", UserController.Instance.ActiveUser, "Test Description", 30, 60, 4,
        //     new List<Instruction> { new Instruction(1, "Step 1"), new Instruction(2, "Step 2") }, dict3, new List<Tag> { new("Tag3"), new("Tag2") }, 2);



        // RecipeController.Instance.CreateRecipe(recipe2);
        // RecipeController.Instance.CreateRecipe(recipe3);

        // UserController.Instance.DeleteAccount("test user", "test user");
        while (true)
        {
            MainMenuOption? chosenOption = ConsoleUtils.GetUserChoice(
                "Choose the next operation", MainMenuOptions);

            if (!chosenOption.HasValue)
            {
                break;
            }

            switch (chosenOption)
            {
                case MainMenuOption.CreateRecipe:
                    CreateRecipe();
                    break;
                case MainMenuOption.UpdateRecipe:
                    EditRecipe();
                    break;
                case MainMenuOption.FilterRecipeSearch:
                    FilterRecipeSearch();
                    break;
                case MainMenuOption.AddRecipeToFavourites:
                    AddRecipeToFavourites();
                    break;
                case MainMenuOption.RateRecipe:
                    RateRecipe();
                    break;
                case MainMenuOption.ViewRecipes:
                    PrintRecipes();
                    break;
                case MainMenuOption.RemoveFromFavourites:
                    RemoveRecipeFromFavourites();
                    break;
                case MainMenuOption.ViewFavoriteRecipes:
                    PrintFavoriteList();
                    break;
                case MainMenuOption.DeleteAccount:
                    Console.WriteLine("Enter your password");
                    string password = Console.ReadLine();
                    UserController.Instance.DeleteAccount(UserController.Instance.ActiveUser.Username, password);
                    LoginOrRegister();
                    break;
                case MainMenuOption.Logout:
                    UserController.Instance.ActiveUser = null;
                    LoginOrRegister();
                    break;
            }
        }
    }

}