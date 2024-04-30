using recipes;

public class Rating
{
    public int? Id {get; set;}
    public int StarRating {get; set;}
    public Recipe Recipe {get; set;}
    public Rating(int rating, Recipe recipe)
    {
        StarRating = rating;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Rating)
            return false;

        return ((Rating)obj).StarRating == StarRating;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StarRating);
    }

    public Rating(){}
}