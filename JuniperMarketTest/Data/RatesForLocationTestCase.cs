using JuniperMarket.Models.Tax;
using JuniperMarket.Services;
using JuniperMarket.Services.Tax;
using JuniperMarketTest.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniperMarketTest.Data
{
    public class RatesForLocationTestCase
    {
        public RatesForLocationTestCase(GetTaxRatesForLocationArgs caseArgs, TaxRates expectedRates, ServiceResultCode expectedResultCode)
        {
            CaseArgs = caseArgs;
            ExpectedRates = expectedRates;
            ExpectedResultCode = expectedResultCode;
        }

        public GetTaxRatesForLocationArgs CaseArgs { get; set; }

        public TaxRates ExpectedRates { get; set; }

        public ServiceResultCode ExpectedResultCode { get; set; }
    }
}
