using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities
{
    public class Shop
    {
        private List<Product> _catalogue;
        public Shop(string name, Address shopAddress)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShopException("The shop must have a name");
            }

            Id = Guid.NewGuid();
            Name = name;
            Address = shopAddress;

            _catalogue = new List<Product>();
        }

        public Guid Id { get; }
        public string Name { get; }
        public Address Address { get; }
        public IReadOnlyList<Product> Catalogue => _catalogue.AsReadOnly();

        public void AddProduct(Product product)
        {
            Product? currProduct = FindProduct(product);

            if (currProduct == null)
            {
                _catalogue.Add(product);
            }
            else
            {
                currProduct.Add(product.Quantity);
            }
        }

        public void RemoveProduct(Product product, uint quantity)
        {
            Product currProduct = GetProduct(product);

            if (currProduct.Quantity < quantity)
            {
                throw new ShopException("Can't remove then exists in the shop");
            }

            currProduct.Remove(quantity);
        }

        public void ChangePrice(Product product, decimal newPrice)
        {
            Product currProduct = GetProduct(product);

            currProduct.SetPrice(newPrice);
        }

        public Product GetProduct(Product product)
        {
            return FindProduct(product) ??
                   throw new ShopException("No such product in the shop");
        }

        public Product? FindProduct(Product product)
        {
            return _catalogue.SingleOrDefault(currProduct => Equals(currProduct.Name, product.Name));
        }
    }
}