using System.Data;
using users;
using recipes;
using System.Globalization;
using filtering;
namespace Ui;

public class Program
{
    private static MainMenuOption[] MainMenuOptions { get; } =
    Enum.GetValues<MainMenuOption>();

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

    private static List<string> FillInstructions()
    {
        Console.WriteLine("Type 'done' when you are finished");
        List<string> instructions = new();
        string input = Console.ReadLine();
        int stepNum = 1;

        while (!input.Equals("done"))
        {
            instructions.Add($"{stepNum}. {input}");
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

    private static Dictionary<Ingredient, double> FillIngredients()
    {
        Console.WriteLine("What ingredients would you like to add?");
        Dictionary<Ingredient, double> ingredients = new();
        string input = "";

        while (!input.Equals("done"))
        {
            Ingredient i = NewIngredient();
            Console.WriteLine("Quantity:");
            double quantity = ValidateDouble();
            ingredients.Add(i, quantity);
            Console.WriteLine("Type 'done' if you are finished");
            input = Console.ReadLine();
        }
        return ingredients;
    }

    // idk if this needs any more implementation
    private static void Login()
    {
        UserController.Instance.ActiveUser = new User("Bianca", new Password("1390238902"));
    }

    private static List<string> FillTags()
    {
        Console.WriteLine("What Tags would you like to add?");
        List<string> tags = new();
        string tag = "";

        while (!tag.Equals("done"))
        {
            Console.WriteLine("Enter your tag, type 'done' if you are finished:");
            tag = Console.ReadLine();
            tags.Add(tag);
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
        Console.WriteLine("List the instrucrions:");
        List<string> instructions = FillInstructions();
        Dictionary<Ingredient, double> ingredients = FillIngredients();
        Console.WriteLine("Add Tags to recipe:");
        List<string> tags = FillTags();
        Console.WriteLine("Budget on a scale of 1-3:");
        int budget = ValidateInt();
        Recipe newRecipe = new Recipe(name, owner, description, prepTimeMins, cookTimeMins, numberOfServings, instructions, ingredients, tags, budget, RecipeController.Instance.AllRecipes.Count);

        RecipeController.Instance.CreateRecipe(newRecipe);
    }

    private static void PrintRecipes()
    {
        int num = 1;
        foreach (Recipe r in RecipeController.Instance.AllRecipes)
        {
            Console.WriteLine($"{num}. {r}");
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
        while(keepAddingIngredients)
        {
            try{
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
        while(true)
        {
            try{
                Console.WriteLine("Enter a keyword:");
                string? keyword = Console.ReadLine();
                if (keyword == null || keyword == "")
                {
                    throw new Exception("Keyword cannot be null or empty");
                }
                Console.WriteLine("Filtering by " + keyword + " will be added. Press Enter to continue");
                Console.ReadLine();
                return keyword;
            }catch (Exception)
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
        while(true)
        {
            try{
                Console.WriteLine("Enter a username who's recipes you would like to search for:");
                string? username = Console.ReadLine();
                if (username == null || username == "")
                {
                    throw new Exception("username cannot be null or empty");
                }
                Console.WriteLine("Filtering by user " + username + " will be added. Press Enter to continue");
                Console.ReadLine();
                return new User(username, new Password("test")); //creates a "fake" user object
            }catch (Exception)
            {
                Console.WriteLine("The entered username cannot be null or empty");
            }
        }
    }

    private static int GetRating()
    {
        while(true)
        {
            try{
                Console.WriteLine("Enter a rating");
                int rating = ValidateInt();
                if (rating > 5 || rating < 1)
                {
                    throw new Exception("rating must be out of 5 stars");
                }
                return rating;
            }catch (Exception){
                Console.WriteLine("The rating must be out of 5 stars");
            }
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
                    break;
                case Ui.FilterRecipeSearch.FilterByTag:
                    break;
                case Ui.FilterRecipeSearch.FilterByTime:
                    break;
                case Ui.FilterRecipeSearch.CompletedFilter:
                    break;
                case Ui.FilterRecipeSearch.RemoveAllFilters:
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

    }

    private static void AddRecipeToFavourites()
    {

    }


    public static void Main()
    {
        var program = new Program();

        Login();

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
            }
        }
    }

}