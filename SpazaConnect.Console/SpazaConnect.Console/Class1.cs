using System;

/// <summary>One item sold in the spaza shop.</summary>
public class Product
{
    public string Name { get; }        // text
    public decimal UnitPrice { get; }  // decimal: exact for money
    public int Quantity { get; }       // whole number of units

    public Product(string name, decimal unitPrice, int quantity)
    {
        // Second line of defence: a Product can never hold bad data
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
        if (unitPrice <= 0) throw new ArgumentException("Price must be above 0.");
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    /// <summary>5(b): stock value = unit price x quantity. RETURNS the value.</summary>
    public decimal CalculateStockValue()
    {
        return UnitPrice * Quantity;
    }
}
