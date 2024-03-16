namespace recipes;
public struct Ingredient
{
    public Ingredient(string name, Measurement measurement, double costPerUnit){
        throw new NotImplementedException();
    }
    public string Name {get; set;}
    public Measurement Measurement {get; set;}
    internal double CostPerUnit {get;}
}