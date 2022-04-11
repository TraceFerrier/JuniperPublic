using JuniperMarket.Models.Shopping;
using JuniperMarket.Models.Tax;
using JuniperMarket.Services.Shopping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniperMarketTest
{
    public class BaseTestClass
    {
        protected T GetService<T>()
        {
            Assert.IsNotNull(m_serviceProvider);
            var service = m_serviceProvider.GetService<T>();
            Assert.IsNotNull(service);
            return service;
        }

        protected async Task<Product> GetRandomProductFromShoppingService()
        {
            var rand = new Random();
            var shoppingService = GetService<IShoppingService>();
            var productsResult = await shoppingService.GetAvailableProducts();
            Assert.IsTrue(productsResult.Status.Succeeded);
            var products = productsResult.List;
            Assert.IsTrue(products.Count > 0);
            return products[rand.Next(0, products.Count - 1)];
        }

        protected void CompareSalesTaxForOrder(SalesTaxForOrder expected, SalesTaxForOrder actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.OrderTotalAmount == actual.OrderTotalAmount);
            Assert.IsTrue(actual.Shipping == expected.Shipping);
            Assert.IsTrue(actual.TaxableAmount == expected.TaxableAmount);
            Assert.IsTrue(actual.AmountToCollect == expected.AmountToCollect);
            Assert.IsTrue(actual.Rate == expected.Rate);
            Assert.IsTrue(actual.HasNexus == expected.HasNexus);
            Assert.IsTrue(actual.FreightTaxable == expected.FreightTaxable);
        }

        protected void CompareTaxRates(TaxRates expected, TaxRates actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.CountryRate == expected.CountryRate);
            Assert.IsTrue(actual.StateRate == expected.StateRate);
            Assert.IsTrue(actual.CountyRate == expected.CountyRate);
            Assert.IsTrue(actual.CityRate == expected.CityRate);
            Assert.IsTrue(actual.CombinedDistrictRate == expected.CombinedDistrictRate);
            Assert.IsTrue(actual.CombinedRate == expected.CombinedRate);

            Assert.IsTrue(actual.FreightTaxable == expected.FreightTaxable);
            Assert.IsTrue(actual.ReducedRate == expected.ReducedRate);
            Assert.IsTrue(actual.SuperReducedRate == expected.SuperReducedRate);
            Assert.IsTrue(actual.ParkingRate == expected.ParkingRate);


            CompareExpectedToActual(expected.Country, actual.Country);
            CompareExpectedToActual(expected.County, actual.County);
            CompareExpectedToActual(expected.City, actual.City);
            CompareExpectedToActual(expected.Name, actual.Name);
        }

        protected void CompareExpectedToActual(string expected, string actual)
        {
            if (expected != null)
            {
                Assert.IsTrue(string.Equals(expected, actual, System.StringComparison.CurrentCultureIgnoreCase));
            }
        }

        protected static ServiceProvider? m_serviceProvider;

    }
}
