namespace filtering;

using System;
using System.Collections.Generic;
using System.Linq;
using recipes;
using users;

public class FilterByOwner : IFilterBy
{
    public FilterByOwner(User owner)
    {
        Owner = owner ?? throw new InvalidOperationException("owner cannot be null");
        Value = owner.Username;
    }
    private User Owner;

    private string value;
    public string Value { get => value; set => this.value = value; }

    public string Name => "Owner";

    /// <summary>
    /// Filters the given recipes by if they are owned by the given owner
    /// </summary>
    /// <param name="recipes">A given list of recipes to filter</param>
    /// <returns>Returns a filtered list of recipes if the given user Owns that recipe</returns>
    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        // List<Recipe> something = recipes.ToList();
        // foreach (Recipe abc in something )
        // {
        //     bool ToF = abc.Owner.Equals(Owner);
        // }
        return from recipe in recipes where recipe.Owner.Username.Equals(Owner.Username) select recipe;
            // .Where(recipe => recipe.Owner.Equals(Owner));
    }

    public override string ToString()
    {
        return "Owner";
    }
}