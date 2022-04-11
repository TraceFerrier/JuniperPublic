using Bogus;
using JuniperMarket.Models.Shopping;
using JuniperMarket.Models.Tax;
using JuniperMarket.Services.Shopping;
using JuniperMarket.Services.Tax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniperMarketTest
{    /// <summary>
     /// System tests that use the live TaxCalculatorService and TaxService.
     /// </summary>
    [TestClass]
    public class SalesTaxForOrderTests : BaseTestClass
    {
        public SalesTaxForOrderTests()
        {
        }

        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            RegisterDependencies();
            AddTestAddresses();
        }

        [TestMethod]
        public async Task CalculateSalesTaxForOrderWithinSameStateUsingTaxCalculatorService()
        {
            await TaxCalculatorTestForSameStatesForShipFromAndShipTo("CA", price: 59.95M, shipping: 1.25M);
            await TaxCalculatorTestForSameStatesForShipFromAndShipTo("WA", price: 259.50M, shipping: 0M);
            await TaxCalculatorTestForSameStatesForShipFromAndShipTo("CO", price: 5.25M, shipping: 0.45M);
            await TaxCalculatorTestForSameStatesForShipFromAndShipTo("NY", price: 5275.99M, shipping: 11.25M);
        }

        [TestMethod]
        public async Task CalculateSalesTaxForOrderBetweenDifferentStatesUsingTaxCalculatorService()
        {
            await TaxCalculatorTestForDifferentStatesForShipFromAndShipTo("CA", "WA");
            await TaxCalculatorTestForDifferentStatesForShipFromAndShipTo("NY", "WA");
            await TaxCalculatorTestForDifferentStatesForShipFromAndShipTo("CA", "CO");
            await TaxCalculatorTestForDifferentStatesForShipFromAndShipTo("NY", "CA");
        }

        [TestMethod]
        public async Task CalculateSalesTaxForOrderThatShipsWithinSameStateUsingTaxService()
        {
            var shippingState = ShoppingFactory.GetRandomState();
            var customer = ShoppingFactory.GenerateCustomer(shippingState);
            var product = ShoppingFactory.GenerateProduct(shippingState);
            await CalculateSalesTaxForOrderUsingTaxService(customer, product);
        }

        [TestMethod]
        public async Task CalculateSalesTaxForOrderThatShipsBetweenDifferentStatesUsingTaxService()
        {
            var customer = ShoppingFactory.GenerateCustomer(SupportedUSAStates.NY);
            var product = ShoppingFactory.GenerateProduct(SupportedUSAStates.CA);
            await CalculateSalesTaxForOrderUsingTaxService(customer, product);
        }

        private async Task CalculateSalesTaxForOrderUsingTaxService(Customer customer, Product product)
        {
            var taxService = GetService<ITaxService>();
            var salesTaxResponse = await taxService.GetSalesTaxForOrder(customer, product);
            Assert.IsNotNull(salesTaxResponse);
            Assert.IsTrue(salesTaxResponse.Status.Succeeded);

            var salesTax = salesTaxResponse.Persistable;
            Assert.IsNotNull(salesTax);
            var shipsFromAddress = product.ShipsFromAddress;
            var shipsToAddress = customer.ShippingAddress;
            if (shipsFromAddress.State == shipsToAddress.State)
            {
                Assert.IsTrue(salesTax.Rate > 0);
                var expectedTax = salesTax.Rate * (decimal.ToSingle(product.Price) + (salesTax.FreightTaxable ? decimal.ToSingle(product.ShippingCost) : 0));
                Assert.IsTrue(Math.Abs(expectedTax - salesTax.AmountToCollect) <= 0.01);
            }
            else
            {
                Assert.IsTrue(salesTax.Rate == 0);
                Assert.IsTrue(salesTax.AmountToCollect == 0);
                Assert.IsTrue(Math.Abs(salesTax.OrderTotalAmount - (decimal.ToSingle(product.Price) + decimal.ToSingle(product.ShippingCost))) <= 0.01);
            }
        }

        private static void AddTestAddress(Address address)
        {
            var state = address.State;
            if (!m_addresses.ContainsKey(state))
            {
                m_addresses.Add(state, new List<Address>());
            }

            m_addresses[state].Add(new Address
            {
                Country = address.Country,
                State = address.State,
                City = address.City,
                StreetAddress = address.StreetAddress,
                ZipCode = address.ZipCode
            });

        }

        private Address GetStateAddress(string state)
        {
            if (!m_addresses.TryGetValue(state, out var addresses))
            {
                throw new ArgumentOutOfRangeException(nameof(state));
            }

            var address = addresses[m_faker.Random.Int(min: 0, max: addresses.Count - 1)];
            return address;
        }

        private async Task<SalesTaxForOrder> CalculateSalesTaxWorker(Address shipFrom, Address shipTo, decimal price, decimal shipping)
        {
            var taxCalculator = GetService<ITaxCalculatorService>();
            var taxService = GetService<ITaxService>();
            var args = BuildSalesTaxForOrderArgs(shipFrom, shipTo, price, shipping);
            var response = await taxCalculator.GetSalesTaxForOrder(args);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status.Succeeded);

            var salesTax = response.Persistable;
            Assert.IsNotNull(salesTax);
            return salesTax;
        }

        private async Task TaxCalculatorTestForSameStatesForShipFromAndShipTo(string state, decimal price, decimal shipping)
        {
            var salesTax = await CalculateSalesTaxWorker(GetStateAddress(state), GetStateAddress(state), price, shipping);
            Assert.IsTrue(salesTax.Rate > 0);
            var expectedTax = salesTax.Rate * (decimal.ToSingle(price) + (salesTax.FreightTaxable ? decimal.ToSingle(shipping) : 0));
            Assert.IsTrue(Math.Abs(expectedTax - salesTax.AmountToCollect) <= 0.02);
        }

        private async Task TaxCalculatorTestForDifferentStatesForShipFromAndShipTo(string stateFrom, string stateTo)
        {
            decimal price = 125;
            decimal shipping = 1.75M;
            var salesTax = await CalculateSalesTaxWorker(GetStateAddress(stateFrom), GetStateAddress(stateTo), price, shipping);
            Assert.IsTrue(salesTax.Rate == 0);
            Assert.IsTrue(salesTax.AmountToCollect == 0);
            Assert.IsTrue(salesTax.OrderTotalAmount == decimal.ToSingle(price) + decimal.ToSingle(shipping));
        }

        private GetSalesTaxForOrderArgs BuildSalesTaxForOrderArgs(Address shipFrom, Address shipTo, decimal price, decimal shipping)
        {
            GetSalesTaxForOrderArgs args = new GetSalesTaxForOrderArgs
            {
                FromCountry = shipFrom.Country,
                FromZip = shipFrom.ZipCode,
                FromState = shipFrom.State,
                FromCity = shipFrom.City,
                FromStreet = shipFrom.StreetAddress,
                ToCountry = shipTo.Country,
                ToState = shipTo.State,
                ToCity = shipTo.City,
                ToStreet = shipTo.StreetAddress,
                ToZip = shipTo.ZipCode,
                Amount = decimal.ToSingle(price),
                Shipping = decimal.ToSingle(shipping),
            };

            args.NexusAddresses.Add(new NexusAddress
            {
                Id = "Main Location",
                Country = shipFrom.Country,
                Zip = shipFrom.ZipCode,
                State = shipFrom.State,
                City = shipFrom.City,
                Street = shipFrom.StreetAddress
            });

            return args;
        }

        static void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IShoppingService, MockShoppingService>();
            serviceCollection.AddSingleton<ITaxService, TaxService>();
            serviceCollection.AddSingleton<ITaxCalculatorService, TaxCalculatorService>();

            m_serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void AddTestAddresses()
        {
            AddTestAddress(new Address { Country = "US", State = "CA", City = "La Jolla", StreetAddress = "9500 Gilman Drive", ZipCode = "92093" });
            AddTestAddress(new Address { Country = "US", State = "CA", City = "La Jolla", StreetAddress = "9500 Gilman Drive", ZipCode = "92093" });
            AddTestAddress(new Address { Country = "US", State = "CA", City = "Rancho Mirage", StreetAddress = "72787 Dinah Shore Dr", ZipCode = "92270" });
            AddTestAddress(new Address { Country = "US", State = "CA", City = "Los Angeles", StreetAddress = "7425 Sunset Blvd", ZipCode = "90046" });
            AddTestAddress(new Address { Country = "US", State = "CA", City = "Los Angeles", StreetAddress = "1335 E 103rd St", ZipCode = "90002" });

            AddTestAddress(new Address { Country = "US", State = "CO", City = "Denver", ZipCode = "80222" });
            AddTestAddress(new Address { Country = "US", State = "CO", City = "Denver", ZipCode = "80210" });
            AddTestAddress(new Address { Country = "US", State = "CO", City = "Fort Collins", ZipCode = "80521" });
            AddTestAddress(new Address { Country = "US", State = "CO", City = "Lafayette", ZipCode = "80026" });

            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "7315 24th Avenue NE", ZipCode = "98115" });
            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "2119 5th Avenue W", ZipCode = "98119" });
            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "2939 Mayfair Avenue N", ZipCode = "98109" });
            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "307 W Highland Drive", ZipCode = "98119" });

            AddTestAddress(new Address { Country = "US", State = "NY", City = "New York", StreetAddress = "32 W 39th St", ZipCode = "10018" });
            AddTestAddress(new Address { Country = "US", State = "NY", City = "Mahopac", ZipCode = "10541" });
            AddTestAddress(new Address { Country = "US", State = "NY", City = "Delmar", ZipCode = "12054" });
        }

        private static Dictionary<string, List<Address>> m_addresses = new Dictionary<string, List<Address>>();
        private static Faker m_faker = new Faker();
    }
}
