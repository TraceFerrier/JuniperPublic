using JuniperMarket.Models.Shopping;
using JuniperMarket.Models.Tax;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public class TaxService : ITaxService
    {
        public TaxService(IEnumerable<ITaxCalculatorService> taxCalculatorServices)
        {
            m_taxCalculatorServices = taxCalculatorServices;
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

            var result = await GetCurrentTaxCalculatorService().GetSalesTaxForOrder(args);
            return result;
        }

        public async Task<ServiceOperationResult<TaxRates>> GetTaxRatesForLocation(GetTaxRatesForLocationArgs args)
        {
            var result = await GetCurrentTaxCalculatorService().GetTaxRatesForLocation(args);
            return result;
        }

        public void OnCustomerSignIn(Customer signingInCustomer)
        {

        }

        private ITaxCalculatorService GetCurrentTaxCalculatorService()
        {
            // For now, we just use the first registered service. In the future, we may need to use
            // a different tax calculator service based on the location of current signed-in user,
            // product ships-from address, etc, and we'll select the most appropriate registered
            // service here.
            foreach (var taxCalculatorService in m_taxCalculatorServices)
            {
                return taxCalculatorService;
            }

            throw new System.ArgumentNullException("No tax calculation service found!");
        }

        private readonly IEnumerable<ITaxCalculatorService> m_taxCalculatorServices;

    }
}
