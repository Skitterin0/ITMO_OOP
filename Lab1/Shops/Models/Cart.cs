using Shops.Exceptions;

namespace Shops.Models
{
    public class Cart
    {
        private List<Product> _products;

        public Cart()
        {
            _products = new List<Product>();
        }

        public IReadOnlyList<Product> Products => _products.AsReadOnly();

        public void Add(Product product)
        {
            Product? currProduct = FindProduct(product);

            if (currProduct == null)
            {
                _products.Add(product);
            }
            else
            {
                currProduct.Add(product.Quantity);
            }
        }

        public void Remove(Product product)
        {
            _products.Remove(product);
        }

        public void Clear()
        {
            _products.Clear();
        }

        private Product? FindProduct(Product product)
        {
            return _products.SingleOrDefault(currProduct => Equals(currProduct, product));
        }
    }
}