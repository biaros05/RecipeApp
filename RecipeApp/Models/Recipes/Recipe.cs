using System.Collections.Generic;
namespace recipes;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using users;
public class Recipe
{
    public Recipe() { }
    //public List<int> UserFavouriteId {get; set;}
    public int OwnerId { get; set; }

    //[ForeignKey("UserFavouriteId")]
    public ICollection<User> UserFavourite { get; set; }

    [ForeignKey("OwnerId")]
    public User Owner { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    private string? _name;

    // sets the name of the recipe, cannot be null or empty + additional validation for length added
    public string Name
    {
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
    private string? _description;

    // sets the description, defaults to the name of the recipe if not provided
    public string Description
    {
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

    // get/set prep time of the recipe in minutes, with max time of 4 hours
    public int PrepTimeMins
    {
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

    // get/set cook time of the recipe in minutes, with max time of 4 hours
    public int CookTimeMins
    {
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
    public string TotalTime
    {
        get
        {
            double mins = this.PrepTimeMins + this.CookTimeMins;
            int hours = (int)Math.Truncate(mins / 60.0);
            mins = mins % 60;
            return $"{hours}h{mins}mins";
        }
    }

    public int NumberOfServings { get; set; }
    public List<Instruction> Instructions { get; set;} = new();
    // contains the ingredient and its quantity for specified unit 
    public List<MeasuredIngredient> Ingredients { get; set; } = new();
    public List<Rating> _ratings {get; set;}= new(); // all the ratings given by users OUT OF FIVE STARS

    // property calculates the average rating with rounding by 2 decimals
    public double? Rating
    {
        get
        {

            double totalRating = 0;
            foreach (Rating r in this._ratings)
            {
                totalRating += r.StarRating;
            }
            double rating = totalRating / (double)_ratings.Count;
            return this._ratings.Count != 0 ? Math.Round(rating, 2) : null;
        }
    } // sum of Ratings and get average.
    public  List<DifficultyRating> _difficulties {get; set;} = new(); // all the difficulties given by users OUT OF 10

    // property calculates the average difficulty with rounding by 2 decimals
    public int? DifficultyRating
    {
        get
        {
            int difficulty = 0;
            foreach (DifficultyRating d in this._difficulties)
            {
                difficulty += d.ScaleRating;
            }
            double averageDiff = difficulty / (double)this._difficulties.Count;
            return this._difficulties.Count != 0 ? (int)Math.Round(averageDiff, 0) : null;
        }

    }

    public List<Tag> Tags { get; set; }
    public string Budget { get; set;}

    // allows user to add rating with necessary validation
    //NOTE - doesnt exist
    public void RateRecipe(int rating, User owner)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 5 stars");
        }
        var context = RecipesContext.Instance;  
        Recipe retrieveRecipe = context.RecipeManager_Recipes.First(r => r.Name.Equals(this.Name) && r.Owner.Equals(this.Owner));
        foreach (Rating r in retrieveRecipe._ratings)
        {
            if (r.Owner == owner)
            {
                throw new Exception("You cannot rate the same recipe twice");
            }
        }
        retrieveRecipe._ratings.Add(new Rating(rating, owner));
        context.SaveChanges();
    }

    // allows the user to rate the difficulty of the recipe with necessary validation
    //NOTE - Doesnt exist
    public void RateDifficulty(int rating, User owner)
    {
        if (rating < 1 || rating > 10)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 10 stars");
        }
        var context = RecipesContext.Instance;
        Recipe retrieveRecipe = context.RecipeManager_Recipes.First(r => r.Id == Id);
        foreach (DifficultyRating r in retrieveRecipe._difficulties)
        {
            if (r.Owner == owner)
            {
                throw new Exception("You cannot rate the same recipe twice");
            }
        }
        retrieveRecipe._difficulties.Add(new DifficultyRating(rating, owner));
        context.SaveChanges();
    }

    // this method will add a tag to the list of tags if it does not already exist
    public void AddTag(string tag)
    {
        Tag t = new(tag);
        if (tag == "")
        {
            throw new ArgumentException("Your tag must have a value");
        }

        var context = RecipesContext.Instance;
        Recipe recipe = context.RecipeManager_Recipes.First(r => r.Id == Id);
        //FIXME - cant get tags
        if (!recipe.Tags.Contains(t))
        {
            recipe.Tags.Add(t);
        }
        context.SaveChanges();
    }

    // will take all parameters for a recipe and update the necessary fields
    //NOTE - never used either
    public void UpdateRecipe(
    string description, int preptimeMins, int cooktimeMins,
    List<MeasuredIngredient> ingredients, List<Tag> tags)
    {
        if (this.Owner != UserController.Instance.ActiveUser)
        {
            throw new ArgumentException("You do not have permission to edit this recipe");
        }
        if (preptimeMins < 0 || preptimeMins > 240)
        {
            throw new ArgumentOutOfRangeException("Prep time cannot be less than 0 or greater than 4 hours");
        }

        if (cooktimeMins < 0 || cooktimeMins > 240)
        {
            throw new ArgumentOutOfRangeException("Cook time cannot be less than 0 or greater than 4 hours");
        }

        var context = RecipesContext.Instance;
        Recipe retrieveRecipe = context.RecipeManager_Recipes.First(r => r.Id == Id);
        retrieveRecipe.Description = description;
        retrieveRecipe.PrepTimeMins = preptimeMins;
        retrieveRecipe.CookTimeMins = cooktimeMins;
        UpdateIngredients(ingredients);
        retrieveRecipe.Tags = tags;
        context.RecipeManager_Recipes.Update(retrieveRecipe);
        context.SaveChanges();
    }

    // helper method that updates the list of ingredients in the recipe and also updates them in 
    // the system if they do not already exist
    private void UpdateIngredients(List<MeasuredIngredient> ingredients)
    {

        List<MeasuredIngredient> newIngredients = new();
        foreach (MeasuredIngredient i in ingredients)
        {
            RecipeController.AddIngredient(i.Ingredient);
            Ingredient? ingredient = RecipesContext.Instance.RecipeManager_Ingredients.FirstOrDefault(ing => ing.Name == i.Ingredient.Name);
            i.Ingredient = ingredient!;
            newIngredients.Add(i);
        }

        this.Ingredients = newIngredients;

    }

    // overriding the equals to check either ID or name + owner
    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Recipe))
        {
            return false;
        }

        return ((Recipe)obj).Name.Equals(this.Name) && ((Recipe)obj).Owner.Equals(this.Owner);
    }

    // copy constructor for Recipe class
    public Recipe(Recipe other)
    : this(other.Name, other.Owner, other.Description, other.PrepTimeMins, other.CookTimeMins, other.NumberOfServings, other.Instructions, other.Ingredients, other.Tags, other.Budget.Length)
    {
        this._ratings = other._ratings;
        this._difficulties = other._difficulties;
    }

    // constructor for recipe with necessary validation
    public Recipe(string name, User owner, string description, int prepTimeMins, int cookTimeMins, int numberOfServings, List<Instruction> instructions, List<MeasuredIngredient> ingredients, List<Tag> tags, int budget)
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

    // gethashcode essential for equality. calculated the hash code by name and ID
    // because those are my attributes of equality for recipe object
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Id, this.Name, this.Owner);
    }

    // tostring override which includes basic information about the recipe
    public override string ToString()
    {
        return $"{this.Name} by {this.Owner}. Rating: {(this.Rating == null ? "Not available" : this.Rating)}, Budget: {this.Budget}";
    }
}