using Shops.Exceptions;

namespace Shops.Models
{
    public class Money : IEquatable<Money>
    {
        private const decimal _noValue = 0;
        private decimal _value;

        public Money(decimal value)
        {
            Value = value;
        }

        public decimal Value
        {
            get => _value;
            private set
            {
                if (value <= _noValue)
                {
                    throw new ShopException("The price must bo more than 0");
                }

                _value = value;
            }
        }

        public void ChangeValue(decimal value)
        {
            Value = value;
        }

        public bool Equals(Money? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}