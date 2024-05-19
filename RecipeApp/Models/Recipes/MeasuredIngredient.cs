using System.ComponentModel;

namespace recipes;
public class MeasuredIngredient : INotifyPropertyChanged
{
    public int? Id {get; set;}
    public Ingredient Ingredient {get; set;}
    public double Quantity {get; set;}
    public Recipe Recipe {get; set;}

    public MeasuredIngredient(Ingredient ingredient, double quantity)
    {
        this.Ingredient = ingredient;
        this.Quantity = quantity;
    }

    public MeasuredIngredient(){}

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is MeasuredIngredient))
            return false;
        return ((MeasuredIngredient)obj).Ingredient.Equals(this.Ingredient) && ((MeasuredIngredient)obj).Quantity == this.Quantity;
    }

    public override string ToString()
    {
        return $"{Ingredient}, {Quantity}";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}