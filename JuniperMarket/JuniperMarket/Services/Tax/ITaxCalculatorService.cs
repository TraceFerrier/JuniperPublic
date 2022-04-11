using JuniperMarket.Models.Tax;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public interface ITaxCalculatorService : ITaxRatesForLocationService
    {
        /// <summary>
        /// Returns the sales tax that should be collected for an order, based, on the source and destination shipping locations.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<ServiceOperationResult<SalesTaxForOrder>> GetSalesTaxForOrder(GetSalesTaxForOrderArgs args);

        /// <summary>
        /// In mocked and development implementations, allows response data to be specified for supported API calls.
        /// </summary>
        /// <param name="dataInjection"></param>
        void InjectData(TaxDataInjection dataInjection);
    }
}
