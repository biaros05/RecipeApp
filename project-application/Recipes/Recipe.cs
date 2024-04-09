using System.Collections.Generic;
namespace recipes;

using System.Timers;
using users;

public class Recipe
{
    private int Id {get;}
    private string _name;
    public string Name{
        get
        {
            return this._name;
        } 
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException("The recipe must have a name");
            }
            else if (value.Length < 10 || value.Length > 100)
            {
                throw new ArgumentOutOfRangeException("The Recipe name must be between 10-100 characters.");
            }
            else
            {
                this._name = value;
            }

        }
    }
    public User Owner {get;}
    private string _description;
    public string Description {
        get
        {
            return this._description;
        }
        set
        {
            if (value == null || value.Equals(""))
            {
                this._description = this.Name;
            }
            else
            {
                this._description = value;
            }
        }
    }

    private int _prepTimeMins;
    public int PrepTimeMins {
        get
        {
            return this._prepTimeMins;
        }
        set
        {
            if (value > 240 || value < 0)
            {
                throw new ArgumentException("The prep time must be less than 4 hours and more than 0 minutes.");
            }
            else 
            {
                this._prepTimeMins = value;
            }
        }
    }

    private int _cookTimeMins;
    public int CookTimeMins {
        get
        {
            return this._cookTimeMins;
        }
        set
        {
            if (value > 240 || value < 0)
            {
                throw new ArgumentException("The cook time must be less than 4 hours and more than 0 minutes.");
            }
            else
            {
                this._cookTimeMins = value;
            }
        }
    }

    // get returns the CookTime + PrepTime
    public int TotalTimeMins{
        get
        {
            double value = (this.PrepTimeMins + this.CookTimeMins) / 60.0;
            return (int) Math.Round(value, 0);
        }
    }

    public int NumberOfServings {get;}
    public List<string> Instructions {get;} = new();
    // contains the ingredient and its quantity for specified unit 
    public Dictionary<Ingredient, double> Ingredients{get;} = new();
    private List<double> Ratings {get;} = new(); // all the ratings given by users OUT OF FIVE STARS

    private double _rating;
    public double Rating {
        get
        {
            int totalRating = 0;
            foreach ( int rating in this.Ratings)
            {
                totalRating += rating;
            } 
            this._rating = totalRating / Ratings.Count;
            return this._rating;
        }
        set
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5 stars");
            }
            this.Ratings.Add(value);
        }
    } // sum of Ratings and get average.
    private List<int> Difficulties {get;} = new(); // all the difficulties given by users OUT OF 10
    private int _difficulty;
    public int DifficultyRating {
        get
        {
            int difficulty = 0;
            foreach (int d in this.Difficulties)
            {
                difficulty += d;
            }
            double averageDiff = difficulty / this.Difficulties.Count;
            return (int) Math.Round(averageDiff, 0);
        }
        set
        {
            if (value < 1 || value > 3)
            {
                throw new ArgumentException("Rating must be between 0 and 5 stars");
            }
            this.Difficulties.Add(value);
        }
    } // sum of Difficulties and get average

    public List<string> Tags {get; set;}

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