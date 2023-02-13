using Shops.Exceptions;

namespace Shops.Models
{
    public class Product
    {
        private Money _price;
        public Product(string name, decimal price, uint quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShopException("The product must have a name");
            }

            (Name, _price, Quantity) = (name, new Money(price), quantity);
        }

        public string Name { get; }
        public uint Quantity { get; private set; }

        public decimal GetPrice()
        {
            return _price.Value;
        }

        public void SetPrice(decimal newPrice)
        {
            _price.ChangeValue(newPrice);
        }

        public void Add(uint quantity)
        {
            Quantity += quantity;
        }

        public void Remove(uint quantity)
        {
            if (Quantity < quantity)
            {
                throw new ShopException("You must remove less products than you have");
            }

            Quantity -= quantity;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Product other)
            {
                return other.Name == Name && other.GetPrice() == _price.Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, _price);
        }
    }
}