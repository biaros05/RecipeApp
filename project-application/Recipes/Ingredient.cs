using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace recipes;
public struct Ingredient
{
    // initializes ingredient with proper data validation
    public Ingredient(string? name, Units? unit)
    {
        if (name == null || unit == null)
        {
            throw new ArgumentNullException("Name and unit cannot be null");
        }
        this.Name = name;
        this.Unit = (Units)unit;
    }
    public string Name { get; set; }
    //private Units _unit;
    public Units Unit
    {
        get; set;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Ingredient))
        {
            return false;
        }
        return ((Ingredient)obj).Name.ToLower() == this.Name.ToLower();
    }

    public override int GetHashCode()
    {
        int hash = 17;//prime num
        unchecked
        {
            hash = hash * 31 + this.Name.GetHashCode();
            return hash;
        }
    }
}