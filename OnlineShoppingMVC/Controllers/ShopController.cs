using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using OnlineShoppingMVC.Extensions;

namespace OnlineShoppingMVC.Controllers;

public class ShopController : Controller
{
    private readonly IMemoryCache _cache;

    public ShopController(IMemoryCache cache)
    {
        _cache = cache;
    }

    // Display products from the online sample API.
    public async Task<IActionResult> Index()
    {
        using HttpClient client = new HttpClient();

        // Get product data asynchronously from Fake Store API.
        var apiProducts = await client.GetFromJsonAsync<List<ApiProduct>>(
            "https://fakestoreapi.com/products");

        // Convert API data to our Product model.
        var products = apiProducts?.Select(p => new Product
        {
            ProductId = p.Id,
            ProductName = p.Title,
            Category = p.Category,
            Price = p.Price,
            // A simple 10% discount is used for the shopping example.
            DiscountPercentage = 10
        }).ToList() ?? new List<Product>();

        return View(products);
    }

    // Product details page.
    // The id is received through the query string.
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Details(int id)
    {
        string cacheKey = "product_" + id;

        // First try to get the product from MemoryCache.
        if (!_cache.TryGetValue(cacheKey, out Product? product))
        {
            using HttpClient client = new HttpClient();

            var apiProducts = await client.GetFromJsonAsync<List<ApiProduct>>(
                "https://fakestoreapi.com/products");

            var apiProduct = apiProducts?.FirstOrDefault(p => p.Id == id);

            if (apiProduct == null)
                return NotFound();

            product = new Product
            {
                ProductId = apiProduct.Id,
                ProductName = apiProduct.Title,
                Category = apiProduct.Category,
                Price = apiProduct.Price,
                DiscountPercentage = 10
            };

            // Cache the product for 10 minutes.
            _cache.Set(cacheKey, product, TimeSpan.FromMinutes(10));
        }

        return View(product);
    }

    // Receive the customer selection.
    [HttpPost]
    public IActionResult SelectProduct(string customerName, string category, int productId)
    {
        // Cookie stores the customer name.
        Response.Cookies.Append("CustomerName", customerName);

        // Session stores the selected category.
        HttpContext.Session.SetString("SelectedCategory", category);

        // The productId comes from the hidden field.
        return RedirectToAction("Details", new { id = productId });
    }

    // Show saved cookie and session values.
    public IActionResult Selection()
    {
        ViewBag.CustomerName =
            Request.Cookies["CustomerName"] ?? "No customer name saved";

        ViewBag.Category =
            HttpContext.Session.GetString("SelectedCategory")
            ?? "No category selected";

        return View();
    }

    // Small class used to read the API response.
    private class ApiProduct
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
        public string Category { get; set; } = "";
    }
}
