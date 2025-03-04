using System;
using System.ComponentModel.DataAnnotations.Schema;
using recipes;
public class Tag
{
    public int? Id {get; set;}
    public string TagName {get; set;}

    public Recipe Recipe {get; set;}

    public Tag(string tag)
    {
        this.TagName = tag;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Tag))
            return false;

        return ((Tag)obj).TagName.ToLower().Equals(this.TagName.ToLower());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TagName);
    }

    public Tag(){}

    public override string ToString()
    {
        return $"{TagName}";
    }

}