using Shops.Entities;
using Shops.Models;

namespace Shops.Service
{
    public interface IShopService
    {
        Shop AddShop(string name, Address address);
        Product AddProduct(Shop shop, Product product);

        Shop? FindShop(Shop shop);
        Shop? FindShop(Product product);
        Product? FindProduct(Product product);
        Shop? FindShopWithCheapestProduct(Product product, uint quantity);
        Product? FindCheapestProduct(Product product, uint quantity);

        void AddToCart(Product product, Customer customer, uint quantity);
        void BuyProduct(Product product, Customer customer, uint quantity);
        void BuyProducts(Customer customer);
    }
}