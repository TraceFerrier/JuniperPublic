using JuniperMarket.Models.Shopping;
using JuniperMarket.Models.Tax;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public interface ITaxService : ITaxRatesForLocationService
    {
        Task<ServiceOperationResult<SalesTaxForOrder>> GetSalesTaxForOrder(Customer purchasingCustomer, Product productForPurchase);
    }
}
