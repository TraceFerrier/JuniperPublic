using JuniperMarket.Models.Tax;
using JuniperMarket.Services;
using JuniperMarket.Services.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuniperMarketTest.Mocks
{
    internal class MockTaxCalculatorService : ITaxCalculatorService
    {
        public async Task<ServiceOperationResult<SalesTaxForOrder>> GetSalesTaxForOrder(GetSalesTaxForOrderArgs args)
        {
            if (args == null || string.IsNullOrWhiteSpace(args.ToCountry))
            {
                return new ServiceOperationResult<SalesTaxForOrder>(ServiceResultCode.FailedInvalidArgs);
            }

            if (m_dataInjection != null && m_dataInjection.SalesTaxForOrderResponse != null)
            {
                await Task.Delay(100);
                return new ServiceOperationResult<SalesTaxForOrder>(m_dataInjection.SalesTaxForOrderResponse);
            }

            return new ServiceOperationResult<SalesTaxForOrder>(ServiceResultCode.FailedObjectNotFound);
        }

        public async Task<ServiceOperationResult<TaxRates>> GetTaxRatesForLocation(GetTaxRatesForLocationArgs args)
        {
            if (args == null || (string.IsNullOrWhiteSpace(args.Zip) && string.IsNullOrWhiteSpace(args.Country)))
            {
                return new ServiceOperationResult<TaxRates>(ServiceResultCode.FailedInvalidArgs);
            }

            if (m_dataInjection != null && m_dataInjection.TaxRatesForLocationResponse != null)
            {
                await Task.Delay(100);
                return new ServiceOperationResult<TaxRates>(m_dataInjection.TaxRatesForLocationResponse);
            }

            return new ServiceOperationResult<TaxRates>(ServiceResultCode.FailedObjectNotFound);
        }

        public void InjectData(TaxDataInjection dataInjection)
        {
            m_dataInjection = dataInjection;
        }

        private TaxDataInjection? m_dataInjection;
    }
}
