using System.Collections.Generic;
namespace recipes;
using users;

public class Recipe
{
    private int Id {get;}
    public string Name{get; set;}
    public User Owner {get;}
    public string? Description {get; set;}
    public int PrepTimeMins {get; set;}
    public int CookTimeMins {get; set;}

    // get returns the CookTime + PrepTime
    public int TotalTimeMins{get;}

    public int NumberOfServings {get; set;}
    public List<string> Instructions {get; set;}
    // contains the ingredient and its quantity for specified unit 
    public Dictionary<Ingredient, double> Ingredients{get; set;} 
    private List<int> Ratings {get; set;} // all the ratings given by users OUT OF FIVE STARS
    public double Rating {get; set;} // sum of Ratings and get average
    private List<int> Difficulties {get; set;} // all the difficulties given by users OUT OF 10
    public double DifficultyRating {get; set;} // sum of Difficulties and get average
    private List<string> Tags {get; set;}

    // add rating to recipe and send information to database
    public void RateRecipe(int rating){}
    
    public void RateDifficulty(int rating){}

    // will take all parameters for a recipe and send this new information to the database (VALDATE THE USER IS THE OWNER)
    public void UpdateDescription(
    string description, int preptimeMins, int cooktimeMins, 
    int servings, List<Ingredient> ingredients, List<string> tags, User owner){}

    // this method will take into account the ingredients and calculate the budget for a given recipe
    public double GetRecipeBudget()
    {
        throw new NotImplementedException();
    }


}