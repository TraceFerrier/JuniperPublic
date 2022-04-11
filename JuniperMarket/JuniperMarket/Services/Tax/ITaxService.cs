using JuniperMarket.Models.Shopping;
using JuniperMarket.Models.Tax;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public interface ITaxService : ITaxRatesForLocationService
    {
        /// <summary>
        /// Notifies the tax service that a new customer has signed into the app.
        /// </summary>
        void OnCustomerSignIn(Customer signingInCustomer);

        /// <summary>
        /// Returns detailed information about the sales tax that should be collected for an order, based on the ShippingAddress 
        /// of the given customer, and the ShipsFromAddress of the given product.
        /// </summary>
        Task<ServiceOperationResult<SalesTaxForOrder>> GetSalesTaxForOrder(Customer purchasingCustomer, Product productForPurchase);

    }
}
