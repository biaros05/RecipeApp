using System.Collections.Generic;
namespace recipes;

using System.Diagnostics.Metrics;
using System.Timers;
using users;

public class Recipe
{
    private int? Id {get;}
    private string? _name;
    public string Name{
        get
        {
            return this._name!;
        } 
        set
        {
            if (string.IsNullOrEmpty(value))
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
    private string? _description;
    public string Description {
        get
        {
            return this._description!;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
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
                throw new ArgumentOutOfRangeException("The prep time must be less than 4 hours and more than 0 minutes.");
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
                throw new ArgumentOutOfRangeException("The cook time must be less than 4 hours and more than 0 minutes.");
            }
            else
            {
                this._cookTimeMins = value;
            }
        }
    }

    // get returns the CookTime + PrepTime and returns proper time formatted string
    public string TotalTime{
        get
        {
            double mins = this.PrepTimeMins + this.CookTimeMins;
            int hours = (int)Math.Truncate(mins / 60.0);
            mins = mins % 60;
            return $"{hours}h{mins}mins";
        }
    }

    public int NumberOfServings {get;}
    public List<string> Instructions {get;} = new();
    // contains the ingredient and its quantity for specified unit 
    public Dictionary<Ingredient, double> Ingredients{get; private set;} = new();
    private readonly List<double> _ratings = new(); // all the ratings given by users OUT OF FIVE STARS

    public double Rating {
        get
        {
            int totalRating = 0;
            foreach ( int r in this._ratings)
            {
                totalRating += r;
            } 
            double rating = totalRating / (double)_ratings.Count;
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
            double averageDiff = difficulty / (double)this._difficulties.Count;
            return (int) Math.Round(averageDiff, 0);
        }
    } // sum of Difficulties and get average


    public List<string> Tags {get; private set;}
    public string Budget {get;}

    // add rating to recipe and send information to database
    public void RateRecipe(int rating)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 5 stars");
        }
        this._ratings.Add(rating);
    }
    
    public void RateDifficulty(int rating)
    {
        if (rating < 1 || rating > 10)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 10 stars");
        }
        this._difficulties.Add(rating);
    }

    // this method will add a tag to the list of tags if it does not already exist
    public void AddTag(string tag)
    {
        if (tag == "")
        {
            throw new ArgumentException("Your tag must have a value");
        }
        if (!this.Tags.Contains(tag))
        {
            this.Tags.Add(tag);
        }
        
    }

    // will take all parameters for a recipe and send this new information to the database (VALDATE THE USER IS THE OWNER)
    public void UpdateRecipe(
    string description, int preptimeMins, int cooktimeMins,
    Dictionary<Ingredient, double> ingredients, List<string> tags)
    {
        if (preptimeMins < 0 || preptimeMins > 240)
        {
            throw new ArgumentOutOfRangeException("Prep time cannot be less than 0 or greater than 4 hours");
        }

        if (cooktimeMins < 0 || cooktimeMins > 240)
        {
            throw new ArgumentOutOfRangeException("Cook time cannot be less than 0 or greater than 4 hours");
        }

        this.Description = description;
        this.PrepTimeMins = preptimeMins;
        this.CookTimeMins = cooktimeMins;
        UpdateIngredients(ingredients);
        this.Tags = tags;

    }

    // helper method that updates the list of ingredients in the recipe and also updates them in 
    // the system if they do not already exist
    private void UpdateIngredients(Dictionary<Ingredient, double> ingredients)
    {
        Dictionary<Ingredient, double> newIngredients = new();
        foreach ((Ingredient i, double quantity) in ingredients)
        {
            if (!newIngredients.Contains(new KeyValuePair<Ingredient, double>(i, quantity)))
            {
                newIngredients.Add(i, quantity);
            }
        }

        this.Ingredients = newIngredients;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Recipe))
        {
            return false;
        }

        return ((Recipe)obj).Id == this.Id || ((Recipe)obj).Name.Equals(this.Name);
    }

    public Recipe(Recipe other)
    : this(other.Name, other.Owner, other.Description, other.PrepTimeMins, other.CookTimeMins, other.NumberOfServings, other.Instructions, other.Ingredients, other.Tags, other.Budget.Length)
    {
    }

    public Recipe(string name, User owner, string description, int prepTimeMins, int cookTimeMins, int numberOfServings, List<String> instructions, Dictionary<Ingredient, double> ingredients, List<string> tags, int budget)
    {
        this.Owner = owner;
        this.Name = name;

        this.Description = description.Equals("") ? this.Name : description;
        this.PrepTimeMins = prepTimeMins;
        this.CookTimeMins = cookTimeMins;
        this.NumberOfServings = numberOfServings;

        if (instructions.Count == 0 || ingredients.Count == 0)
        {
            throw new ArgumentException("there must be a minimum of one ingredient and one instruction on each recipe");
        }

        this.Instructions = instructions;
        UpdateIngredients(ingredients);
        this.Tags = tags;

        if (budget < 1 || budget > 3)
        {
            throw new ArgumentException("Budget rating must be 1, 2, or 3");
        }
        this.Budget = new string('$', budget);
    }

    // gethashcode essential for equality. i have calculated the hash code by name and Id 
    // because those are my attributes of equality for recipe object
    public override int GetHashCode()
    {
        int hash = 17;//prime num
        unchecked {
            hash = hash * 31 + this.Id.GetHashCode();
            hash = hash * 31 + (Name?.GetHashCode() ?? 0);
            return hash;
        }
    }
}