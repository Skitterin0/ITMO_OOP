using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Service
{
    public class ShopService : IShopService
    {
        private List<Shop> _shops;

        public ShopService()
        {
            _shops = new List<Shop>();
        }

        public Shop AddShop(string name, Address address)
        {
            var newShop = new Shop(name, address);
            _shops.Add(newShop);

            return newShop;
        }

        public Product AddProduct(Shop shop, Product product)
        {
            Shop? currShop = FindShop(shop);

            if (currShop == null)
            {
                throw new ShopException("No such shop was found in the service");
            }

            currShop.AddProduct(product);

            return product;
        }

        public Shop? FindShop(Shop shop)
        {
            return _shops.SingleOrDefault(currShop => currShop.Id == shop.Id);
        }

        public Shop? FindShop(Product product)
        {
            return _shops.FirstOrDefault(shop => shop.FindProduct(product)?.Equals(product) ?? false);
        }

        public Product? FindProduct(Product product)
        {
            return _shops.SelectMany(shop => shop.Catalogue)
                .FirstOrDefault(currProduct => Equals(currProduct, product));
        }

        public Product GetProduct(Product product)
        {
            return FindProduct(product) ??
                   throw new ShopException("No such product in the service");
        }

        public Shop? FindShopWithCheapestProduct(Product product, uint quantity)
        {
            Product cheapestProduct = FindCheapestProduct(product, quantity)
                                      ?? throw new ShopException("No such product type in the shop");
            return FindShop(cheapestProduct);
        }

        public Product? FindCheapestProduct(Product product, uint quantity)
        {
            return _shops.SelectMany(shop => shop.Catalogue)
                .Where(currProduct => Equals(currProduct.Name, product.Name))
                .OrderBy(currProduct => currProduct.GetPrice())
                .FirstOrDefault(currProduct => currProduct.Quantity >= quantity);
        }

        public void AddToCart(Product product, Customer customer, uint quantity)
        {
            Product currProduct = GetProduct(product);

            if (currProduct.Quantity < quantity)
            {
                throw new ShopException("Not enough products to add");
            }

            customer.Cart.Add(new Product(currProduct.Name, currProduct.GetPrice(), quantity));
        }

        public void BuyProduct(Product product, Customer customer,  uint quantity)
        {
            Product currProduct = GetProduct(product);

            decimal totalPrice = currProduct.GetPrice() * quantity;

            if (customer.Wallet.Value < totalPrice)
            {
                throw new ShopException("Not enough money to buy products");
            }

            currProduct.Remove(quantity);
            customer.Pay(totalPrice);
        }

        public void BuyProducts(Customer customer)
        {
            foreach (Product cartProduct in customer.Cart.Products)
            {
                BuyProduct(cartProduct, customer, cartProduct.Quantity);
            }
        }
    }
}