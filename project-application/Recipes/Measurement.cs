namespace recipes;
using recipes;
using System;

public class Measurement {
    public Measurement(double ratioToBase, double ratioFromBase, Units unit){}
    // this will convert the measurement to base unit, and the base from the given measurement
    // make sure the enums are the same
    public double ConvertTo(Measurement toUnit, double quantity)
    {
        throw new NotImplementedException();
    }

    private double ConvertToBase(double quantity) // quantity * first ratio
    {
        throw new NotImplementedException();
    }
    private double ConvertFromBase(double quantity) // quantity * second ratio
    {
        throw new NotImplementedException();
    }
    // type of unit
    public Units Unit {get; set;}

}