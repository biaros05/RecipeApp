namespace recipes;
using recipes;
using System;

public class Measurement {
    private readonly double _ratioToBase;
    private readonly double _ratioFromBase;

    // type of unit
    public Units Unit {get;}
    public Measurement(Units unit, double ratioToBase=0, double ratioFromBase=0)
    {
        this.Unit = unit;
        this._ratioToBase = unit == Units.Quantity ? 0 : ratioToBase;
        this._ratioFromBase = unit == Units.Quantity ? 0 : ratioFromBase;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Measurement))
        {
            return false;
        }
        return ((Measurement)obj).Unit == this.Unit;
    }
    
    // this will convert the measurement to base unit, and then from this, will convert 
    //the base to the new measurement given
    // it will make sure the enums are the same, or else will not convert.
    public double ConvertTo(Measurement toUnit, double quantity)
    {
        if (toUnit.Unit != this.Unit)
        {
            throw new ArgumentException("These measurements do not match");
        }
        if (quantity < 0)
        {
            throw new ArgumentException("Quantity cannot be negative");
        }
        double baseQuantity = this.ConvertToBase(quantity);
        double convertedQuantity = toUnit.ConvertFromBase(baseQuantity);
        return Math.Round(convertedQuantity, 2);

    }

    private double ConvertToBase(double quantity) // quantity * first ratio
    {
        return this.Unit == Units.Quantity ? quantity : quantity * this._ratioToBase;
    }
    private double ConvertFromBase(double quantity) // quantity * second ratio
    {
        return this.Unit == Units.Quantity ? quantity : quantity * this._ratioFromBase;
    }

    public override int GetHashCode()
    {
        return this.Unit.GetHashCode();
    }
}