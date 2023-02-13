using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities
{
    public class Customer
    {
        private const int _noValue = 0;
        private Money _wallet;
        public Customer(string name, decimal wallet)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShopException("Customer must have a name");
            }

            Name = name;
            _wallet = new Money(wallet);
            Cart = new Cart();
        }

        public string Name { get; }
        public Money Wallet => _wallet;
        public Cart Cart { get; }

        public void Pay(decimal value)
        {
            if (value < _noValue)
            {
                throw new ShopException("The payment must be more than 0");
            }

            _wallet.ChangeValue(_wallet.Value - value);
        }

        public void Deposit(decimal value)
        {
            if (value < _noValue)
            {
                throw new ShopException("The deposit must be more than 0");
            }

            _wallet.ChangeValue(_wallet.Value + value);
        }
    }
}