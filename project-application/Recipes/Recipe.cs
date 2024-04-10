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
    private readonly List<double> _ratings = new(); // all the ratings given by users OUT OF FIVE STARS

    public double Rating {
        get
        {
            int totalRating = 0;
            foreach ( int r in this._ratings)
            {
                totalRating += r;
            } 
            double rating = totalRating / _ratings.Count;
            return Math.Round(rating, 2);
        }
    } // sum of Ratings and get average.
    private readonly List<int> _difficulties = new(); // all the difficulties given by users OUT OF 10
    public int DifficultyRating {
        get
        {
            int difficulty = 0;
            foreach (int d in this._difficulties)
            {
                difficulty += d;
            }
            double averageDiff = difficulty / this._difficulties.Count;
            return (int) Math.Round(averageDiff, 0);
        }
    } // sum of Difficulties and get average


    public List<string> Tags {get;}

    // add rating to recipe and send information to database
    public void RateRecipe(int rating)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5 stars");
        }
        this._ratings.Add(rating);
    }

    public string Budget {get;}
    
    public void RateDifficulty(int rating)
    {
        if (rating < 1 || rating > 3)
        {
            throw new ArgumentException("Rating must be between 1 and 5 stars");
        }
        this._difficulties.Add(rating);
    }

    // this method will add a tag to the list of tags
    public void AddTag(string tag)
    {
        if (tag == "")
        {
            throw new ArgumentException("Your tag must have a value");
        }
    }

    // will take all parameters for a recipe and send this new information to the database (VALDATE THE USER IS THE OWNER)
    public void UpdateDescription(
    string description, int preptimeMins, int cooktimeMins, 
    int servings, List<Ingredient> ingredients, List<string> tags, User owner){}

    // this method will take into account the ingredients and calculate the budget for a given recipe
    public double GetRecipeBudget()
    {
        throw new NotImplementedException();
    }

    public Recipe(int budget)
    {
        if (budget < 1 || budget > 3)
        {
            throw new ArgumentException("Budget rating must be 1, 2, or 3");
        }
        this.Budget = new string('$', budget);
    }


}