using System.Collections.Generic;

public class Recipe{
    private int Id {get;}
    private string Name{get; set;}
    private User Owner {get;}
    private string? Description {get; set;}
    private int PrepTimeMins {get; set;}
    private int CookTimeMins {get; set;}

    // get returns the CookTIme + PrepTime
    private int TotalTimeMins{get;}

    private int NumberOfServings {get; set;}
    private List<string> Instructions {get; set;}
    private List<Ingredient> Ingredients{get; set;}
    private int[] Ratings {get; set;}
    public double Rating{get; set;} // sum of Ratings and get average
    private List<string> Tags {get; set;}
    // update rating accordingly
    public void RateRecipe(int rating){}
    public void UpdateDescription(string description, int preptimeMins, int cooktimeMins, int servings, List<Ingredient> ingredients, List<string> tags){}


}