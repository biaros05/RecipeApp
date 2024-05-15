using System;
using users;
namespace recipes;
public class DifficultyRating
{
    public int? Id {get; set;}
    public int ScaleRating {get; set;}
    public User Owner {get; set;}
    public Recipe Recipe {get; set;}

    
    public DifficultyRating(int rating, User owner)
    {
        ScaleRating = rating;
        Owner = owner;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not DifficultyRating)
            return false;

        return ((DifficultyRating)obj).ScaleRating == ScaleRating;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ScaleRating);
    }

    public DifficultyRating(){}
}