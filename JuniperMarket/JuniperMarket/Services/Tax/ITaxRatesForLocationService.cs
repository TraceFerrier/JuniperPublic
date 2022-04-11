using JuniperMarket.Models.Tax;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public interface ITaxRatesForLocationService
    {
        /// <summary>
        /// Returns a breakdown of the sales tax rates for a given location.
        /// </summary>
        Task<ServiceOperationResult<TaxRates>> GetTaxRatesForLocation(GetTaxRatesForLocationArgs args);
    }
}
