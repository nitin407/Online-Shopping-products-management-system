namespace OnlineShoppingMVC.Models;

// This model represents a product in the shopping system.
public class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public string Category { get; set; } = "";

    public decimal Price { get; set; }

    public decimal DiscountPercentage { get; set; }
}
