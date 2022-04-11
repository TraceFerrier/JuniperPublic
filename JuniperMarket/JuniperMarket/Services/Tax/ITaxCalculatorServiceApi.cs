using JuniperMarket.Models.Tax;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    [Headers("Content-Type: application/json", "Authorization: Bearer")]
    public interface ITaxCalculatorServiceApi
    {
        [Get("/rates/{zipCode}")]
        Task<TaxRateResponse> GetTaxRatesForLocation(string zipCode, GetTaxRatesForLocationOptionalArgs args);

        [Post("/taxes")]
        Task<SalesTaxForOrderResponse> GetSalesTaxForOrder([Body] GetSalesTaxForOrderArgs args);
    }
}
