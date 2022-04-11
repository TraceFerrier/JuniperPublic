using JuniperMarket.Models.Tax;
using JuniperMarket.Services.Shopping;
using JuniperMarket.Services.Tax;
using JuniperMarketTest.Data;
using JuniperMarketTest.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace JuniperMarketTest
{
    /// <summary>
    /// Unit tests that run without live service dependencies.
    /// </summary>
    [TestClass]
    public class MockTaxCalculatorTests : BaseTestClass
    {
        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            RegisterDependencies();
        }

        [TestMethod]
        public async Task AssureTaxServiceUsageOfTaxCalculatorForGetSalesTaxForOrder()
        {
            var taxCalculatorService = GetService<ITaxCalculatorService>();
            var taxService = GetService<ITaxService>();

            float rate = 0.075f;
            float taxableAmount = 75.99f;
            var expectedResponseData = new TaxDataInjection
            {
                SalesTaxForOrderResponse = new SalesTaxForOrder
                {
                    OrderTotalAmount = 27.75f,
                    Shipping = 1.25f,
                    TaxableAmount = taxableAmount,
                    AmountToCollect = taxableAmount * rate,
                    Rate = rate,
                    HasNexus = false,
                    FreightTaxable = false,
                }
            };
            taxCalculatorService.InjectData(expectedResponseData);

            var shippingState = ShoppingFactory.GetRandomState();
            var customer = ShoppingFactory.GenerateCustomer(shippingState);
            var product = ShoppingFactory.GenerateProduct(shippingState);

            var actualTaxResponse = await taxService.GetSalesTaxForOrder(customer, product);
            Assert.IsTrue(actualTaxResponse.Status.Succeeded);
            CompareSalesTaxForOrder(expectedResponseData.SalesTaxForOrderResponse, actualTaxResponse.Persistable);
        }

        [TestMethod]
        public async Task AssureTaxServiceUsageOfTaxCalculatorForGetTaxRatesForLocation()
        {
            var taxCalculatorService = GetService<ITaxCalculatorService>();
            var taxService = GetService<ITaxService>();

            float stateRate = 0.065f;
            float countyRate = 0.003f;
            float cityRate = 0.01f;
            var expectedResponseData = new TaxDataInjection
            {
                TaxRatesForLocationResponse = new TaxRates
                {
                    Zip = TestZipCodes.PalmDesertOne,
                    Country = "US",
                    CountryRate = 0,
                    State = "CA",
                    StateRate = stateRate,
                    CountyRate = countyRate,
                    CityRate = cityRate,
                    CombinedRate = stateRate + countyRate + cityRate,
                    FreightTaxable = false,
                }
            };
            taxCalculatorService.InjectData(expectedResponseData);

            var args = new GetTaxRatesForLocationArgs
            {
                Zip = TestZipCodes.PalmDesertOne
            };

            var actualTaxResponse = await taxService.GetTaxRatesForLocation(args);
            Assert.IsTrue(actualTaxResponse.Status.Succeeded);
            CompareTaxRates(expectedResponseData.TaxRatesForLocationResponse, actualTaxResponse.Persistable);
        }

        static void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IShoppingService, MockShoppingService>();
            serviceCollection.AddSingleton<ITaxService, TaxService>();
            serviceCollection.AddSingleton<ITaxCalculatorService, MockTaxCalculatorService>();

            m_serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
