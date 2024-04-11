namespace recipes;
public struct Ingredient
{
    // initializes ingredient with proper data validation
    public Ingredient(string name, Measurement measurement){
        // MAKE SURE IT DOES NOT ALREADY EXIST
        this.Name = name;
        this.Measurement = measurement;
    }
    public string Name {get; set;}
    public Measurement Measurement {get; set;}
    // cost per unit for the base unit we will choose (hopefully this logic will work) 
}