namespace OnlineShoppingMVC.Extensions;

// This class contains an extension method for Product.
public static class ProductExtensions
{
    // Calculate the price after applying the discount.
    public static decimal GetDiscountedPrice(this Product product)
    {
        decimal discount = product.Price * product.DiscountPercentage / 100;
        return product.Price - discount;
    }
}
