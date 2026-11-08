using System;
using System.Collections.Generic;
using System.Globalization;

class Program
{
    const int MinProducts = 5;                       // assessment minimum
    static readonly List<Product> products = new();  // all captured products

    // 5(h): one place for every log/output message
    static void Log(string level, string message)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}");
    }

    // 5(f): name must not be empty; keep asking until it is valid
    static string ReadName()
    {
        while (true)
        {
            Console.Write("Product name: ");
            string input = (Console.ReadLine() ?? "").Trim();
            if (input.Length > 0) return input;
            Log("ERROR", "Name cannot be empty.");
        }
    }

    // 5(f): price must be a number above 0 (accepts 12.50 and 12,50)
    static decimal ReadPrice()
    {
        while (true)
        {
            Console.Write("Unit price (R): ");
            string raw = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
            if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
            { Log("ERROR", "Price must be a number, for example 12.50."); continue; }
            if (price <= 0)
            { Log("ERROR", "Price must be greater than 0."); continue; }
            return price;
        }
    }

    // 5(f): quantity must be a whole number, 0 or more
    static int ReadQuantity()
    {
        while (true)
        {
            Console.Write("Quantity in stock: ");
            string raw = (Console.ReadLine() ?? "").Trim();
            if (!int.TryParse(raw, out int qty))
            { Log("ERROR", "Quantity must be a whole number, for example 10."); continue; }
            if (qty < 0)
            { Log("ERROR", "Quantity cannot be negative."); continue; }
            return qty;
        }
    }

    // 5(c): capture products until we have at least MinProducts
    static void CaptureProducts()
    {
        while (true)
        {
            Console.WriteLine($"\n--- Product {products.Count + 1} ---");
            var product = new Product(ReadName(), ReadPrice(), ReadQuantity());
            products.Add(product);
            Log("INFO", $"Added '{product.Name}'. Products captured: {products.Count}.");

            if (products.Count >= MinProducts)
            {
                Console.Write("Add another product? (y/n): ");
                if ((Console.ReadLine() ?? "").Trim().ToLower() != "y") break;
            }
            else
            {
                Log("INFO", $"{MinProducts - products.Count} more needed (minimum is {MinProducts}).");
            }
        }
    }

    // 5(c): readable table of all products
    static void DisplayProducts()
    {
        if (products.Count == 0) { Log("WARN", "No products yet. Choose option 1 first."); return; }
        Console.WriteLine($"\n{"#",-3}{"Product",-22}{"Price",10}{"Qty",7}{"Stock value",14}");
        Console.WriteLine(new string('-', 56));
        for (int i = 0; i < products.Count; i++)
        {
            Product p = products[i];
            Console.WriteLine($"{i + 1,-3}{p.Name,-22}{"R" + p.UnitPrice.ToString("F2"),10}{p.Quantity,7}{"R" + p.CalculateStockValue().ToString("F2"),14}");
        }
    }

    // 5(d): iterate the collection and accumulate the total
    static decimal CalculateTotalStockValue()
    {
        decimal total = 0;                          // start at zero
        foreach (Product p in products)
        {
            total += p.CalculateStockValue();       // reuse the 5(b) method
        }
        return total;
    }

    static void ShowTotalStockValue()
    {
        if (products.Count == 0) { Log("WARN", "No products yet. Choose option 1 first."); return; }
        Console.WriteLine($"\nTotal value of all stock: R{CalculateTotalStockValue():F2}");
    }

    // 5(e): initialise, iterate, compare. No sorting.
    static Product? FindHighestValueProduct()
    {
        if (products.Count == 0) return null;

        Product highest = products[0];                       // 1. initialise with the first item
        for (int i = 1; i < products.Count; i++)             // 2. iterate through the rest
        {
            if (products[i].CalculateStockValue() > highest.CalculateStockValue())  // 3. compare
            {
                highest = products[i];                       // new champion
            }
        }
        return highest;
    }

    static void ShowHighestValueProduct()
    {
        Product? best = FindHighestValueProduct();
        if (best == null) { Log("WARN", "No products yet. Choose option 1 first."); return; }
        Console.WriteLine($"\nHighest stock value: {best.Name} at R{best.CalculateStockValue():F2}");
    }

    static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;  // always use . for decimals
        Log("INFO", "SpazaConnect Stock & Sales Manager started.");

        bool running = true;
        while (running)                                              // iteration controls the menu
        {
            Console.WriteLine("\n===== SpazaConnect Menu =====");
            Console.WriteLine("1. Capture products");
            Console.WriteLine("2. Display products");
            Console.WriteLine("3. Total stock value");
            Console.WriteLine("4. Highest-value product");
            Console.WriteLine("5. Exit");
            Console.Write("Choose 1-5: ");

            switch ((Console.ReadLine() ?? "").Trim())               // selection chooses the action
            {
                case "1": CaptureProducts(); break;
                case "2": DisplayProducts(); break;
                case "3": ShowTotalStockValue(); break;
                case "4": ShowHighestValueProduct(); break;
                case "5": running = false; Log("INFO", "Goodbye."); break;
                default: Log("ERROR", "Invalid choice. Enter a number from 1 to 5."); break;
            }
        }
    }
}
