using System.Data;
using users;
using recipes;
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
        while (!validInput && num >=0)
        {
            Console.WriteLine("Enter a number:");
            string? input = Console.ReadLine();
            validInput = int.TryParse(input, out num);
        }
        return num;
    }

    private static double ValidateDouble()
    {
        double num = 0; 
        bool validInput = false;
        while (!validInput && num >=0)
        {
            Console.WriteLine("Enter a number:");
            string? input = Console.ReadLine();
            validInput = double.TryParse(input, out num);
        }
        return num;
    }

    private static List<string> FillInstructions()
    {
        List<string> instructions = new();
        string input = Console.ReadLine();
        int stepNum = 1;

        while ((!input.Equals("done"))) {
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
                "Choose the next operation", UnitsOptions);

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
            try{
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
        Dictionary<Ingredient, double> ingredients = new();
        string input = Console.ReadLine();
        double quantity = ValidateDouble();

        while (!input.Equals("done")) 
        {
            input = Console.ReadLine();
            Ingredient i = NewIngredient();
            quantity = ValidateDouble();

        }
        return ingredients;
    }

    private static void Login()
    {

    }

    private static List<string> FillTags()
    {
        throw new NotImplementedException();
    }

    private static void CreateRecipe()
    {
        string name = Console.ReadLine();
        User owner = UserController.ActiveUser; 
        string description = Console.ReadLine();
        int prepTimeMins = ValidateInt();
        int cookTimeMins = ValidateInt(); 
        int numberOfServings = ValidateInt(); 
        List<String> instructions = FillInstructions(); 
        Dictionary<Ingredient, double> ingredients = FillIngredients(); 
        List<string> tags = FillTags();
        int budget = 0;
    }

    private static void FilterRecipeSearch()
    {
        
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
            }
        }
    }

}