// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

// Interface defining vending machine operations
public interface IVending
{
    void Purchase(int productId);
    List<string> ShowAll();
    string Details(int productId);
    void InsertMoney(int amount);
    Dictionary<int, int> EndTransaction();
}

// Abstract Product class
public abstract class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }

    public abstract string Examine();
    public abstract string Use();
}

// Drink class inheriting from Product
public class Drink : Product
{
    public int Volume { get; set; } // Unique property

    public override string Examine()
    {
        return $"Id: {Id}, Name: {Name}, Cost: {Cost}kr, Volume: {Volume}ml";
    }

    public override string Use()
    {
        return $"You drink the {Name}.";
    }
}

// Snack class inheriting from Product
public class Snack : Product
{
    public int Weight { get; set; } // Unique property

    public override string Examine()
    {
        return $"Id: {Id}, Name: {Name}, Cost: {Cost}kr, Weight: {Weight}g";
    }

    public override string Use()
    {
        return $"You eat the {Name}.";
    }
}

// Toy class inheriting from Product
public class Toy : Product
{
    public string Material { get; set; } // Unique property

    public override string Examine()
    {
        return $"Id: {Id}, Name: {Name}, Cost: {Cost}kr, Material: {Material}";
    }

    public override string Use()
    {
        return $"You play with the {Name}.";
    }
}

// VendingMachineService implementing IVending interface
public class VendingMachineService : IVending
{
    private readonly List<Product> products = new List<Product>();
    private readonly List<int> validDenominations = new List<int> { 1, 5, 10, 20, 50, 100, 500, 1000 };
    private int moneyPool = 0;

    public VendingMachineService()
    {
        // Initialize some products
        products.Add(new Drink { Id = 1, Name = "Coca-Cola", Cost = 15, Volume = 330 });
        products.Add(new Snack { Id = 2, Name = "Chips", Cost = 20, Weight = 150 });
        products.Add(new Toy { Id = 3, Name = "Action Figure", Cost = 50, Material = "Plastic" });
    }

    public void Purchase(int productId)
    {
        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }

        if (moneyPool >= product.Cost)
        {
            moneyPool -= product.Cost;
            Console.WriteLine($"Purchased {product.Name}. {product.Use()}");
        }
        else
        {
            Console.WriteLine("Not enough money. Please insert more money.");
        }
    }

    public List<string> ShowAll()
    {
        return products.Select(p => $"Id: {p.Id}, Name: {p.Name}, Cost: {p.Cost}kr").ToList();
    }

    public string Details(int productId)
    {
        var product = products.FirstOrDefault(p => p.Id == productId);
        return product?.Examine() ?? "Product not found.";
    }

    public void InsertMoney(int amount)
    {
        if (!validDenominations.Contains(amount))
        {
            throw new ArgumentException("Invalid denomination.");
        }
        moneyPool += amount;
        Console.WriteLine($"Inserted {amount}kr. Current balance: {moneyPool}kr.");
    }

    public Dictionary<int, int> EndTransaction()
    {
        var change = new Dictionary<int, int>();
        foreach (var denomination in validDenominations.OrderByDescending(d => d))
        {
            int count = moneyPool / denomination;
            if (count > 0)
            {
                change[denomination] = count;
                moneyPool %= denomination;
            }
        }

        Console.WriteLine("Transaction ended. Change returned: " + string.Join(", ", change.Select(c => $"{c.Value}x{c.Key}kr")));
        return change;
    }
}

// Main program to run the console application
class Program
{
    static void Main(string[] args)
    {
        IVending vendingMachine = new VendingMachineService();

        while (true)
        {
            Console.WriteLine("\nWelcome to the Vending Machine!");
            Console.WriteLine("");
            Console.WriteLine("1. Show all products");
            Console.WriteLine("");
            Console.WriteLine("2. Insert money");
            Console.WriteLine("");
            Console.WriteLine("3. Buy product");
            Console.WriteLine("");
            Console.WriteLine("4. Show product details");
            Console.WriteLine("");
            Console.WriteLine("5. End transaction");
            Console.WriteLine("");
            Console.WriteLine("6. Exit");
            Console.WriteLine("");
            Console.Write("Select an option: ");
            int choice = int.Parse(Console.ReadLine());

            try
            {
                switch (choice)
                {
                    case 1:
                        var products = vendingMachine.ShowAll();
                        products.ForEach(Console.WriteLine);
                        break;
                    case 2:
                        Console.Write("Insert amount (valid denominations: 1, 5, 10, 20, 50, 100, 500, 1000): ");
                        int amount = int.Parse(Console.ReadLine());
                        vendingMachine.InsertMoney(amount);
                        break;
                    case 3:
                        var Allproducts = vendingMachine.ShowAll();
                        Allproducts.ForEach(Console.WriteLine);

                        Console.Write("Enter product Id to buy: ");
                        int productId = int.Parse(Console.ReadLine());
                        vendingMachine.Purchase(productId);
                        break;
                    case 4:
                        Console.Write("Enter product Id for details: ");
                        productId = int.Parse(Console.ReadLine());
                        Console.WriteLine(vendingMachine.Details(productId));
                        break;
                    case 5:
                        var change = vendingMachine.EndTransaction();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

