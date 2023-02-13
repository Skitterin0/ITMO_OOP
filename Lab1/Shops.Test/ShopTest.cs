using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Service;
using Xunit;
using Xunit.Abstractions;

namespace Shops.Test
{
    public class ShopTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ShopService _service = new ShopService();

        public ShopTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void AddProductsToTheShop_AbleToBuyProductsAndQuantityChanges()
        {
            var customer = new Customer("Vadim", 10000);
            Shop itmoStore = _service.AddShop("ITMO.Store", new Address("Lomonosova", 9));

            Product tshirt = _service.AddProduct(itmoStore, new Product("T-shirt", 1900, 14));
            Product hoodie = _service.AddProduct(itmoStore, new Product("Hoodie", 2490, 25));

            Assert.Contains(tshirt, itmoStore.Catalogue);
            Assert.Contains(hoodie, itmoStore.Catalogue);

            uint tshirtsBefore = tshirt.Quantity;
            uint hoodiesBefore = hoodie.Quantity;

            _service.BuyProduct(tshirt, customer, 2);
            _service.BuyProduct(hoodie, customer, 1);

            Assert.Equal(tshirtsBefore, tshirt.Quantity + 2);
            Assert.Equal(hoodiesBefore, hoodie.Quantity + 1);

            Assert.Equal(10000, customer.Wallet.Value + 3800 + 2490);
        }

        [Fact]
        public void SetAndChangePriceToProductInShop()
        {
            Shop itmoStore = _service.AddShop("ITMO.Store", new Address("Lomonosova", 9));

            Product tshirt = _service.AddProduct(itmoStore, new Product("T-shirt", 1900, 14));
            Product hoodie = _service.AddProduct(itmoStore, new Product("Hoodie", 2400, 25));

            decimal tshirtPriceBefore = tshirt.GetPrice();
            decimal hoodiePriceBefore = hoodie.GetPrice();

            _service.GetProduct(tshirt).SetPrice(1700);
            _service.GetProduct(hoodie).SetPrice(2000);

            Assert.Equal(tshirt.GetPrice() + 200, tshirtPriceBefore);
            Assert.Equal(hoodie.GetPrice() + 400, hoodiePriceBefore);
        }

        [Fact]
        public void NotEnoughProductToBuy()
        {
            var customer = new Customer("Vadim", 10000);
            Shop itmoStore = _service.AddShop("ITMO.Store", new Address("Lomonosova", 9));

            Product tshirt = _service.AddProduct(itmoStore, new Product("T-shirt", 1900, 14));

            Assert.Throws<ShopException>(() => _service.BuyProduct(tshirt, customer, 15));
        }

        [Fact]
        public void NotEnoughMoneyToBuyProducts()
        {
            var customer = new Customer("Vadim", 1);
            Shop itmoStore = _service.AddShop("ITMO.Store", new Address("Lomonosova", 9));

            Product tshirt = _service.AddProduct(itmoStore, new Product("T-shirt", 1900, 14));
            Product hoodie = _service.AddProduct(itmoStore, new Product("Hoodie", 2490, 25));

            _service.AddToCart(tshirt, customer, 2);
            _service.AddToCart(hoodie, customer, 1);

            Assert.Throws<ShopException>(() => _service.BuyProducts(customer));
        }

        [Fact]
        public void FindTheCheapestProductInService()
        {
            Shop itmoStore = _service.AddShop("ITMO.Store", new Address("Lomonosova", 9));
            Shop otherStore = _service.AddShop("otherStore", new Address("SomeAddress", 11));

            Product hoodieItmo = _service.AddProduct(itmoStore, new Product("hoodie", 2000, 12));
            Product otherHoodie = _service.AddProduct(itmoStore, new Product("hoodie", 2400, 12));

            Assert.Equal(hoodieItmo, _service.FindCheapestProduct(otherHoodie, 1));
        }
    }
}