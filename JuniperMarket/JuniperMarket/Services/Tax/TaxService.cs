using JuniperMarket.Models.Shopping;
using JuniperMarket.Models.Tax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public class TaxService : ITaxService
    {
        public TaxService(ITaxCalculatorService taxCalculatorService)
        {
            m_taxCalculatorService = taxCalculatorService;
        }

        public async Task<ServiceOperationResult<SalesTaxForOrder>> GetSalesTaxForOrder(Customer purchasingCustomer, Product productForPurchase)
        {
            GetSalesTaxForOrderArgs args = new GetSalesTaxForOrderArgs
            {
                FromCountry = productForPurchase.ShipsFromAddress.Country,
                FromZip = productForPurchase.ShipsFromAddress.ZipCode,
                FromState = productForPurchase.ShipsFromAddress.State,
                FromCity = productForPurchase.ShipsFromAddress.City,
                FromStreet = productForPurchase.ShipsFromAddress.StreetAddress,
                ToCountry = purchasingCustomer.ShippingAddress.Country,
                ToState = purchasingCustomer.ShippingAddress.State,
                ToCity = purchasingCustomer.ShippingAddress.City,
                ToStreet = purchasingCustomer.ShippingAddress.StreetAddress,
                ToZip = purchasingCustomer.ShippingAddress.ZipCode,
                Amount = decimal.ToSingle(productForPurchase.Price),
                Shipping = decimal.ToSingle(productForPurchase.ShippingCost)
            };

            args.NexusAddresses.Add(new NexusAddress
            {
                Id = "Main Location",
                Country = productForPurchase.ShipsFromAddress.Country,
                Zip = productForPurchase.ShipsFromAddress.ZipCode,
                State = productForPurchase.ShipsFromAddress.State,
                City = productForPurchase.ShipsFromAddress.City,
                Street = productForPurchase.ShipsFromAddress.StreetAddress
            });

            var result = await m_taxCalculatorService.GetSalesTaxForOrder(args);
            return result;
        }

        public async Task<ServiceOperationResult<TaxRates>> GetTaxRatesForLocation(GetTaxRatesForLocationArgs args)
        {
            var result = await m_taxCalculatorService.GetTaxRatesForLocation(args);
            return result;
        }

        private readonly ITaxCalculatorService m_taxCalculatorService;

    }
}
