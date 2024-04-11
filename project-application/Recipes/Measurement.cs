namespace recipes;
using recipes;
using System;

public class Measurement {
    private readonly double _ratioToBase;
    private readonly double _ratioFromBase;
    private Units unit;
    public Measurement(Units unit, double ratioToBase=0, double ratioFromBase=0)
    {
        this.Unit = unit;
        this._ratioToBase = ratioToBase;
        this._ratioFromBase = ratioFromBase;
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
        double baseQuantity = this.ConvertToBase(quantity);
        double convertedQuantity = toUnit.ConvertFromBase(baseQuantity);
        return convertedQuantity;

    }

    private double ConvertToBase(double quantity) // quantity * first ratio
    {
        return quantity * this._ratioToBase;
    }
    private double ConvertFromBase(double quantity) // quantity * second ratio
    {
        return quantity * this._ratioFromBase;
    }
    // type of unit
    public Units Unit {get; set;}

}