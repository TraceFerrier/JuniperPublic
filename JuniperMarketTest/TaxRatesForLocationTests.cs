using JuniperMarket.Models.Tax;
using JuniperMarket.Services;
using JuniperMarket.Services.Tax;
using JuniperMarketTest.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace JuniperMarketTest
{
    /// <summary>
    /// System tests that use the live TaxCalculatorService and TaxService.
    /// </summary>
    [TestClass]
    public class TaxRatesForLocationTests : BaseTestClass
    {
        public TaxRatesForLocationTests()
        {
        }

        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            RegisterDependencies();
        }

        #region ExecuteTaxRatesForUnitedStatesValidLocationZipOnly
        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesValidLocationZipOnlyUsingTaxCalculatorService()
        {
            await ExecuteTaxRatesForUnitedStatesValidLocationZipOnly(GetService<ITaxCalculatorService>());
        }

        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesValidLocationZipOnlyUsingTaxService()
        {
            await ExecuteTaxRatesForUnitedStatesValidLocationZipOnly(GetService<ITaxService>());
        }

        private async Task ExecuteTaxRatesForUnitedStatesValidLocationZipOnly(ITaxRatesForLocationService taxRatesForLocationService)
        {
            var testCase = new RatesForLocationTestCase(
                new GetTaxRatesForLocationArgs
                {
                    Zip = TestZipCodes.SeattleOne
                },
                new TaxRates
                {
                    Zip = TestZipCodes.SeattleOne,
                    StateRate = 0.065f,
                    CountyRate = 0.003f,
                    CityRate = 0.0115f,
                    CombinedDistrictRate = 0.023f,
                    CombinedRate = 0.1025f,
                    FreightTaxable = true
                },
                ServiceResultCode.Ok
                );

            await ExecuteTestCase(taxRatesForLocationService, testCase);
        }
        #endregion

        #region ExecuteTaxRatesForUnitedStatesInvalidLocationZipOnly
        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesInvalidLocationZipOnlyUsingTaxCalculatorService()
        {
            await ExecuteTaxRatesForUnitedStatesInvalidLocationZipOnly(GetService<ITaxCalculatorService>());
        }

        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesInvalidLocationZipOnlyUsingTaxService()
        {
            await ExecuteTaxRatesForUnitedStatesInvalidLocationZipOnly(GetService<ITaxService>());
        }

        private async Task ExecuteTaxRatesForUnitedStatesInvalidLocationZipOnly(ITaxRatesForLocationService taxRatesForLocationService)
        {
            var testCase = new RatesForLocationTestCase(
                new GetTaxRatesForLocationArgs
                {
                    Zip = TestZipCodes.InvalidUsaZip
                },
                new TaxRates
                {
                },
                ServiceResultCode.FailedObjectNotFound
                );

            await ExecuteTestCase(taxRatesForLocationService, testCase);
        }
        #endregion

        #region ExecuteTaxRatesForUnitedStatesNoZipLocationZipOnly
        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesNoZipLocationZipOnlyUsingTaxCalculatorService()
        {
            await ExecuteTaxRatesForUnitedStatesNoZipLocationZipOnly(GetService<ITaxCalculatorService>());
        }

        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesNoZipLocationZipOnlyUsingTaxService()
        {
            await ExecuteTaxRatesForUnitedStatesNoZipLocationZipOnly(GetService<ITaxService>());
        }

        private async Task ExecuteTaxRatesForUnitedStatesNoZipLocationZipOnly(ITaxRatesForLocationService taxRatesForLocationService)
        {
            var testCase = new RatesForLocationTestCase(
                new GetTaxRatesForLocationArgs
                {
                    Zip = TestZipCodes.NullZip
                },
                new TaxRates
                {
                },
                ServiceResultCode.FailedInvalidArgs
                );

            await ExecuteTestCase(taxRatesForLocationService, testCase);
        }
        #endregion

        #region
        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesLocationWithAllParamsUsingTaxCalculatorService()
        {
            await ExecuteTaxRatesForUnitedStatesLocationWithAllParams(GetService<ITaxCalculatorService>());
        }

        [TestMethod]
        public async Task GetTaxRatesForUnitedStatesLocationWithAllParamsUsingTaxService()
        {
            await ExecuteTaxRatesForUnitedStatesLocationWithAllParams(GetService<ITaxService>());
        }

        private async Task ExecuteTaxRatesForUnitedStatesLocationWithAllParams(ITaxRatesForLocationService taxRatesForLocationService)
        {
            var testCase = new RatesForLocationTestCase(
                new GetTaxRatesForLocationArgs
                {
                    Country = "US",
                    Zip = TestZipCodes.SeattleTwo,
                    State = "WA",
                    City = "Seattle",
                    Street = "102 Pike St"
                },
                new TaxRates
                {
                    Zip = TestZipCodes.SeattleOne,
                    StateRate = 0.065f,
                    CountyRate = 0.003f,
                    CityRate = 0.0115f,
                    CombinedDistrictRate = 0.023f,
                    CombinedRate = 0.1025f,
                    FreightTaxable = true
                },
                ServiceResultCode.Ok
                );

            await ExecuteTestCase(taxRatesForLocationService, testCase);
        }
        #endregion ExecuteTaxRatesForInternationalCountry

        #region ExecuteTaxRatesForInternationalCountry
        [TestMethod]
        public async Task GetTaxRatesForInternationalCountryUsingTaxCalculatorService()
        {
            await ExecuteTaxRatesForInternationalCountry(GetService<ITaxCalculatorService>());
        }

        [TestMethod]
        public async Task GetTaxRatesForInternationalCountryUsingTaxService()
        {
            await ExecuteTaxRatesForInternationalCountry(GetService<ITaxService>());
        }

        private async Task ExecuteTaxRatesForInternationalCountry(ITaxRatesForLocationService taxRatesForLocationService)
        {
            var testCase = new RatesForLocationTestCase(
                new GetTaxRatesForLocationArgs
                {
                    Country = "FI",
                },
                new TaxRates
                {
                    Country = "FI",
                    Name = "Finland",
                    ParkingRate = 0.0M,
                    ReducedRate = 0.14M,
                    StandardRate = 0.24M,
                    SuperReducedRate = 0.1M,
                    FreightTaxable = true
                },
                ServiceResultCode.Ok
                );

            await ExecuteTestCase(taxRatesForLocationService, testCase);
        }
        #endregion

        #region ExecuteTaxRatesForInternationalCountryMoreArgs
        [TestMethod]
        public async Task GetTaxRatesForInternationalCountryMoreArgsUsingTaxCalculatorService()
        {
            await ExecuteTaxRatesForInternationalCountryMoreArgs(GetService<ITaxCalculatorService>());
        }

        [TestMethod]
        public async Task GetTaxRatesForInternationalCountryMoreArgsUsingTaxService()
        {
            await ExecuteTaxRatesForInternationalCountryMoreArgs(GetService<ITaxService>());
        }

        private async Task ExecuteTaxRatesForInternationalCountryMoreArgs(ITaxRatesForLocationService taxRatesForLocationService)
        {
            var testCase = new RatesForLocationTestCase(
                new GetTaxRatesForLocationArgs
                {
                    Country = "FR",
                    City = "Paris"
                },
                new TaxRates
                {
                    Country = "FR",
                    Name = "France",
                    ParkingRate = 0.0M,
                    ReducedRate = 0.1M,
                    StandardRate = 0.2M,
                    SuperReducedRate = 0.055M,
                    FreightTaxable = true
                },
                ServiceResultCode.Ok
                );

            await ExecuteTestCase(taxRatesForLocationService, testCase);
        }
        #endregion

        private async Task ExecuteTestCase(ITaxRatesForLocationService taxRatesForLocationService, RatesForLocationTestCase testCase)
        {
            var result = await ExecuteTaxRatesForLocation(taxRatesForLocationService, testCase.CaseArgs, testCase.ExpectedResultCode);
            if (result.Status.Succeeded)
            {
                CompareTaxRates(testCase.ExpectedRates, result.Persistable);
            }
        }

        private async Task<ServiceOperationResult<TaxRates>> ExecuteTaxRatesForLocation(
            ITaxRatesForLocationService taxRatesForLocationService,
            GetTaxRatesForLocationArgs args, 
            ServiceResultCode expectedCode = ServiceResultCode.Ok)
        {
            ServiceOperationResult<TaxRates>? result = await taxRatesForLocationService.GetTaxRatesForLocation(args);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status.ResultCode == expectedCode);
            return result;
        }

        static void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ITaxService, TaxService>();
            serviceCollection.AddSingleton<ITaxCalculatorService, TaxCalculatorService>();

            m_serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
