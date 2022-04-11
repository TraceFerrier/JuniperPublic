using JuniperMarket.Models.Tax;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Services.Tax
{
    public interface IDataInjection
    {
        void InjectSalesTaxForOrderResponse(SalesTaxForOrder salesTaxForOrder);

        void InjectTaxRatesForLocationResponse(TaxRates taxRates);
    }

    public class TaxDataInjection
    {
        public SalesTaxForOrder SalesTaxForOrderResponse { get; set; }

        public TaxRates TaxRatesForLocationResponse { get; set; }
    }
}
