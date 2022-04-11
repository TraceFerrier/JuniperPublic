using Bogus;
using JuniperMarket.Extensions;
using JuniperMarket.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Shopping
{
    public partial class ShoppingFactory
    {
        /// <summary>
        /// Generates a random customer, whose shipping address will be a random valid address within the given state.
        /// You can pass a value from GetRandomState if you want the shipping address to also be randomized to any state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Customer GenerateCustomer(SupportedUSAStates state)
        {
            var person = new Person();
            var customer = new Customer
            {
                FullName = string.Format("{0} {1}", person.FirstName, person.LastName),
                ProfileImageUrl = person.Avatar,
                HomeAddress = new Address
                {
                    City = person.Address.City,
                    State = person.Address.State,
                    StreetAddress = person.Address.Street,
                    ZipCode = person.Address.ZipCode,
                },
                ShippingAddress = GetCustomerAddress(state),
                Bio = m_faker.Lorem.Paragraph(min: 3).EndWithPeriod(),
            };

            return customer;
        }

        /// <summary>
        /// Generates a random product, whose ShipsFromAddress will be a random valid address within the given state.
        /// You can pass a value from GetRandomState if you want the ShipsFromAddress to also be randomized to any state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Product GenerateProduct(SupportedUSAStates state)
        {
            var productFaker = GetProductFaker(state);
            return productFaker.Generate();
        }

        public static List<Product> GenerateProducts(int minCount, int maxCount, SupportedUSAStates state)
        {
            var faker = GetProductFaker(state);

            return faker.GenerateBetween(minCount, maxCount);
        }

        private static Faker<Product> GetProductFaker(SupportedUSAStates state)
        {
            const int imageWidth = 200;
            const int imageHeight = 200;
            var faker = new Faker<Product>().CustomInstantiator(f => new Product())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => GetRandomProductDescription(f))
                .RuleFor(p => p.ProductImageUrl, f => GetRandomImage(f, imageWidth, imageHeight))
                .RuleFor(p => p.QuantityAvailable, f => f.Random.Int(min: 1, max: 200))
                .RuleFor(p => p.Price, f => GetRandomProductPrice(f))
                .RuleFor(p => p.ShippingCost, f => GetRandomShippingCost(f))
                .RuleFor(p => p.ShipsFromAddress, GetBusinessAddress(state));

            return faker;
        }

        private static Address GetBusinessAddress(SupportedUSAStates state)
        {
            return GetStateAddress(state);
        }

        private static Address GetCustomerAddress(SupportedUSAStates state)
        {
            return GetStateAddress(state);
        }

        private static Address GetStateAddress(SupportedUSAStates state)
        {
            EnsureAddresses();
            string stateCode = state.ToString();
            if (!m_addresses.TryGetValue(stateCode, out var addresses))
            {
                throw new ArgumentOutOfRangeException(nameof(state));
            }

            var address = addresses[m_faker.Random.Int(min: 0, max: addresses.Count - 1)];
            return address;
        }

        public static SupportedUSAStates GetRandomState()
        {
            Array states = Enum.GetValues(typeof(SupportedUSAStates));
            return (SupportedUSAStates) states.GetValue(m_faker.Random.Int(min: 0, max: states.Length - 2));
        }

        private static string GetRandomProductDescription(Faker faker)
        {
            var desc = faker.Commerce.ProductDescription().EndWithPeriod();
            return desc;
        }

        private static string GetRandomImage(Faker faker, int width, int height)
        {
            var imageUrl = faker.Image.PicsumUrl(width: width, height: height);
            return imageUrl;
        }

        private static decimal GetRandomProductPrice(Faker faker)
        {
            int majorPrice = faker.Random.Int(min: 1, max: 800);
            int minorPriceIndex = GetDiminishingRandomInt(faker, 0, m_minorPrices.Count - 1);
            int minorPrice = m_minorPrices[minorPriceIndex];
            string priceStr = string.Format("{0}.{1}", majorPrice, minorPrice);
            var price = decimal.Parse(priceStr);
            return price;
        }

        private static decimal GetRandomShippingCost(Faker faker)
        {
            int majorCost = GetDiminishingRandomInt(faker, 1, 8);
            int minorCostIndex = GetDiminishingRandomInt(faker, 0, m_minorPrices.Count - 1);
            int minorCost = m_minorPrices[minorCostIndex];
            string costStr = string.Format("{0}.{1}", majorCost, minorCost);
            var cost = decimal.Parse(costStr);
            return cost;
        }

        private static int GetDiminishingRandomInt(Faker faker, int minValue, int maxValue)
        {
            return (int)Math.Floor(Math.Abs(faker.Random.Double(0, 1) - faker.Random.Double(0, 1)) * (1 + maxValue - minValue) + minValue);
        }

        private static void EnsureAddresses()
        {
            if (m_addresses.Count == 0)
            {
                BuildAddresses();
            }
        }

        private static void BuildAddresses()
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
            AddTestAddress(new Address { Country = "US", State = "CO", City = "Westminster", ZipCode = "80035" });
            AddTestAddress(new Address { Country = "US", State = "CO", City = "Castle Rock", ZipCode = "80109" });
            AddTestAddress(new Address { Country = "US", State = "CO", City = "Watkins", ZipCode = "80137" });

            AddTestAddress(new Address { Country = "US", State = "FL", City = "Belleview", StreetAddress = "5516 Abshier Blvd", ZipCode = "10018" });
            AddTestAddress(new Address { Country = "US", State = "FL", City = "Cocoa Beach", StreetAddress = "26 South Atlantic Avenue", ZipCode = "32931" });
            AddTestAddress(new Address { Country = "US", State = "FL", City = "Daytona Beach", StreetAddress = "1191 Beville Rd", ZipCode = "32119" });
            AddTestAddress(new Address { Country = "US", State = "FL", City = "Miami", StreetAddress = "1051 NW 14th Street", ZipCode = "33136" });

            AddTestAddress(new Address { Country = "US", State = "KS", City = "Abilene", StreetAddress = "2201 N Buckeye Ave", ZipCode = "67410" });
            AddTestAddress(new Address { Country = "US", State = "KS", City = "Manhattan", StreetAddress = "101 E Bluemont Ave", ZipCode = "66502" });
            AddTestAddress(new Address { Country = "US", State = "KS", City = "Wichita", StreetAddress = "1016 W Douglas", ZipCode = "67203" });

            AddTestAddress(new Address { Country = "US", State = "NY", City = "New York", StreetAddress = "32 W 39th St", ZipCode = "10018" });
            AddTestAddress(new Address { Country = "US", State = "NY", City = "Mahopac", ZipCode = "10541" });
            AddTestAddress(new Address { Country = "US", State = "NY", City = "Delmar", ZipCode = "12054" });

            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "7315 24th Avenue NE", ZipCode = "98115" });
            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "2119 5th Avenue W", ZipCode = "98119" });
            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "2939 Mayfair Avenue N", ZipCode = "98109" });
            AddTestAddress(new Address { Country = "US", State = "WA", City = "Seattle", StreetAddress = "307 W Highland Drive", ZipCode = "98119" });



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

        private static Dictionary<string, List<Address>> m_addresses = new Dictionary<string, List<Address>>();
        private static List<int> m_minorPrices = new List<int> { 99, 95, 00, 75, 50, 25, 15 };
        private static Faker m_faker = new Faker();

    }
}
