namespace filtering;

using System;
using System.Collections.Generic;
using System.Linq;
using recipes;


public class FilterByTags : IFilterBy
{
    public FilterByTags(List<Tag> tags)
    {
        if ((tags == null) || tags.Count == 0)
        {
            throw new InvalidOperationException("tags cannot be null or empty");
        }
        Tags = tags;
        foreach (Tag i in tags)
        {
             Value += i.TagName +", ";
        }
        Value = Value[..Value.LastIndexOf(", ")];
    }
    private List<Tag> Tags;

    private string value;
    public string Value { get => value; set => this.value = value; }

    public string Name => "Tags";

    private bool ContainsTag(List<Tag> recipeTags)
    {
        foreach (Tag tag in recipeTags)
        {
            if (Tags.Contains(tag))
            {
                return true;
            }
        }
        return false;
    }

    public IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes)
    {
        List<Recipe> recipesList = recipes.ToList();
        var query = recipesList
            .Where(recipe => ContainsTag(new List<Tag>(recipe.Tags)));
        return query.AsQueryable();
        //NOTE - avalonia can't handle this??? gives me error when using intersect
        // return recipes
        //     .Where(recipe => recipe.Tags.Intersect(Tags).Any());
    }

    public override string ToString()
    {
        return "Tags";
    }
}