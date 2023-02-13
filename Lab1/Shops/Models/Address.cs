using Shops.Exceptions;

namespace Shops.Models;

public class Address : IEquatable<Address>
{
    public Address(string streetName, uint homeNumber)
    {
        if (string.IsNullOrWhiteSpace(streetName))
        {
            throw new ShopException("The street must have a name");
        }

        StreetName = streetName;
        HomeNumber = homeNumber;
    }

    public string StreetName { get; }
    public uint HomeNumber { get; }

    public bool Equals(Address? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return StreetName == other.StreetName && HomeNumber == other.HomeNumber;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Address)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StreetName, HomeNumber);
    }
}