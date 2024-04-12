using recipes;

namespace project_application_test;

[TestClass]
public class MeasurementsTests
{
    [TestMethod]
    public void EqualsMethod_QuantityEqual_ReturnsTrue()
    {
        Measurement m = new(Units.Quantity);
        Measurement m2 = new(Units.Quantity);

        Assert.AreEqual(m, m2);
    }


    [TestMethod]
    public void EqualsMethod_UnitNotEqual_ReturnsFalse()
    {
        Measurement m = new(Units.Quantity);
        Measurement m2 = new(Units.Mass);

        Assert.AreNotEqual(m, m2);
    }

    [TestMethod]
    public void ConvertTo_QuantityUnit_ReturnsSameQuantity()
    {
        Measurement m = new(Units.Quantity);
        Measurement m2 = new(Units.Quantity);

        double newQuantity = m.ConvertTo(m2, 9);

        Assert.AreEqual(9, newQuantity);
    }

    [TestMethod]
    public void ConvertTo_MassUnit_ReturnsConverted()
    {
        Measurement mg = new(Units.Mass, 0.001, 1000);
        Measurement g = new(Units.Mass, 1, 1);

        double newQuantity = mg.ConvertTo(g, 9530.90);

        Assert.AreEqual(9.53, newQuantity);
    }

    [TestMethod]
    public void ConvertTo_UnitDontMatch_ThrowsException()
    {
        Measurement mg = new(Units.Mass, 0.001, 1000);
        Measurement L = new(Units.Volume, 1, 1);

        Action act = () => mg.ConvertTo(L, 9530.90);

        Assert.ThrowsException<ArgumentException>(act, "Units do not match");
    }

    [TestMethod]
    public void ConvertTo_NegativeQuantity_ThrowsException()
    {
        Measurement mg = new(Units.Mass, 0.001, 1000);
        Measurement L = new(Units.Volume, 1, 1);

        Action act = () => mg.ConvertTo(L, -19302);

        Assert.ThrowsException<ArgumentException>(act, "Quantity cannot be negative");
    }


    // convert from measurement to base unit
    // convert from base unit to measurement
    // convert from measurment to another measurement
    // convert from measurment to another measurement if measurements enums dont match
    // convert from measurment to another measurement if quantity null

}