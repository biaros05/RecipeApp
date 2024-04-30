public class DifficultyRating
{
    public int? Id {get; set;}
    public int ScaleRating {get; set;}
    public DifficultyRating(int rating)
    {
        ScaleRating = rating;
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