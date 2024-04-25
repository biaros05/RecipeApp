namespace recipes;
public class MeasuredIngredient
{
    public int? Id {get; set;}
    public Ingredient Ingredient {get; set;}
    public double Quantity {get; set;}

    public MeasuredIngredient(Ingredient ingredient, double quantity)
    {
        this.Ingredient = ingredient;
        this.Quantity = quantity;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is MeasuredIngredient))
            return false;
        return ((MeasuredIngredient)obj).Ingredient.Equals(this.Ingredient) && ((MeasuredIngredient)obj).Quantity == this.Quantity;
    }
}