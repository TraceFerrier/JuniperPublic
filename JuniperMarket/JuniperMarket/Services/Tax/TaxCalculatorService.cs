using JuniperMarket.Models.Tax;
using Refit;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Tax
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        public TaxCalculatorService()
        {
            var settings = new RefitSettings();
            settings.AuthorizationHeaderValueGetter = GetApiKey;
            m_serviceApi = RestService.For<ITaxCalculatorServiceApi>("https://api.taxjar.com/v2", settings);
        }

        public async Task<ServiceOperationResult<SalesTaxForOrder>> GetSalesTaxForOrder(GetSalesTaxForOrderArgs args)
        {
            if (args == null || string.IsNullOrWhiteSpace(args.ToCountry))
            {
                return new ServiceOperationResult<SalesTaxForOrder>(ServiceResultCode.FailedInvalidArgs);
            }

            try
            {
                var response = await m_serviceApi.GetSalesTaxForOrder(args);
                return new ServiceOperationResult<SalesTaxForOrder>(response.TaxForOrder);
            }

            catch (ApiException ex)
            {
                return new ServiceOperationResult<SalesTaxForOrder>(ex.StatusCode, ex.Message);
            }

            catch (Exception ex)
            {
                return new ServiceOperationResult<SalesTaxForOrder>(ex);
            }
        }

        public async Task<ServiceOperationResult<TaxRates>> GetTaxRatesForLocation(GetTaxRatesForLocationArgs args)
        {
            if (args == null || (string.IsNullOrWhiteSpace(args.Zip) && string.IsNullOrWhiteSpace(args.Country)))
            {
                return new ServiceOperationResult<TaxRates>(ServiceResultCode.FailedInvalidArgs);
            }

            try
            {
                string zipCode = args.Zip;
                var apiArgs = new GetTaxRatesForLocationOptionalArgs(args);

                var taxRateResponse = await m_serviceApi.GetTaxRatesForLocation(args.Zip, apiArgs);
                return new ServiceOperationResult<TaxRates>(taxRateResponse.Rate);
            }
            catch (ApiException ex)
            {
                return new ServiceOperationResult<TaxRates>(ex.StatusCode, ex.Message);
            }

            catch (Exception ex)
            {
                return new ServiceOperationResult<TaxRates>(ex);
            }

        }
        Task<string> GetApiKey()
        {
            return Task.FromResult("5da2f821eee4035db4771edab942a4cc");
        }

        public void InjectData(TaxDataInjection dataInjection)
        {
            // The real service doesn't accept injection, so a throw is appropriate here.
            throw new NotImplementedException();
        }

        private ITaxCalculatorServiceApi m_serviceApi;
    }
}
