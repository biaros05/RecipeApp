namespace recipes;
public struct Ingredient
{
    // initializes ingredient with proper data validation
    public Ingredient(string name, Measurement measurement, double costPerUnit){
        throw new NotImplementedException();
    }
    public string Name {get; set;}
    public Measurement Measurement {get; set;}
    // cost per unit for the base unit we will choose (hopefully this logic will work) 
    internal double CostPerUnit {get;}
}