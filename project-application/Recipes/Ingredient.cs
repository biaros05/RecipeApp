using System.Diagnostics.CodeAnalysis;

namespace recipes;
public struct Ingredient
{
    // initializes ingredient with proper data validation
    public Ingredient(string name, Units unit){
        // MAKE SURE IT DOES NOT ALREADY EXIST
        this.Name = name;
        this.Unit = unit;
    }
    public string Name {get; set;}
    public Units Unit {get; set;}

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Ingredient))
        {
            return false;
        }   
        return ((Ingredient)obj).Name.ToLower() == this.Name.ToLower() &&
                ((Ingredient)obj).Unit == this.Unit;
    }
}