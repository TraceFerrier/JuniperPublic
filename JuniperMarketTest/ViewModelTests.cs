using JuniperMarket.Models.Shopping;
using JuniperMarket.Services;
using JuniperMarket.Services.Shopping;
using JuniperMarket.Services.Tax;
using JuniperMarket.ViewModels.Shop;
using JuniperMarketTest.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using JuniperMarket.Extensions;

namespace JuniperMarketTest
{
    [TestClass]
    public class ViewModelTests : BaseTestClass
    {
        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            RegisterDependencies();
        }

        [TestMethod]
        public void ShopViewModelNoProductsBeforeViewAppearance()
        {
            var viewModel = GetService<ShopPageViewModel>();
            Assert.IsTrue(viewModel.ShoppingListViewModels.Count == 1);
            Assert.IsInstanceOfType(viewModel.ShoppingListViewModels[0], typeof(ShopLoadingCellViewModel));
        }

        [TestMethod]
        public async Task ShopViewModelShouldLoadProductsOnViewAppearance()
        {
            var shoppingService = GetService<IShoppingService>();
            var viewModel = GetService<ShopPageViewModel>();
            await viewModel.OnViewAppearing(isFirstAppearance: true);
            var productsResult = await shoppingService.GetAvailableProducts();
            Assert.IsTrue(productsResult.Status.Succeeded);
            Assert.IsTrue(viewModel.ShoppingListViewModels.Count > 0);
            Assert.IsTrue(viewModel.ShoppingListViewModels.Count >= productsResult.List.Count);
            Assert.IsNotInstanceOfType(viewModel.ShoppingListViewModels[0], typeof(ShopLoadingCellViewModel));
        }

        [TestMethod]
        public async Task ShopViewModelShouldShowErrorOnAvailableProductsFailure()
        {
            var shoppingService = GetService<IShoppingService>();
            var viewModel = GetService<ShopPageViewModel>();
            shoppingService.InjectErrorResult(ServiceResultCode.FailedObjectNotFound);
            await viewModel.OnViewAppearing(isFirstAppearance: true);
            shoppingService.DisableErrorResult();
            Assert.IsTrue(viewModel.ShoppingListViewModels.Count == 1);
            Assert.IsInstanceOfType(viewModel.ShoppingListViewModels[0], typeof(ShopErrorExperienceCellViewModel));
        }

        [TestMethod]
        public async Task ProductDetailBuyProductTest()
        {
            var product = await GetRandomProductFromShoppingService();
            var productDetailsViewModel = GetService<ProductDetailViewModel>();
            productDetailsViewModel.ProductId = product.Id;
            var customerId = productDetailsViewModel.CurrentCustomer.Id;

            await productDetailsViewModel.OnViewAppearing(isFirstAppearance: true);
            Assert.IsTrue(productDetailsViewModel.ProductId == product.Id);

            // Before placing an order, product shouldn't be on orders list.
            var shoppingService = GetService<IShoppingService>();
            ServiceOperationResult<Order> ordersResult = await shoppingService.GetCustomerOrders(new GetUserOrdersArgs { CustomerId = customerId });
            Assert.IsTrue(ordersResult.Status.Succeeded);
            Assert.IsFalse(ordersResult.List.ContainsProduct(product.Id));

            // Start the buy process, but confirm that the product doesn't get ordered until confirmation (normally be the user) is received.
            await productDetailsViewModel.StartBuyProduct();
            Assert.IsTrue(productDetailsViewModel.HasBeenOrdered == false);

            // Finish the buy, and verify that the order is now on the orders list.
            await productDetailsViewModel.FinishBuyProduct();
            Assert.IsTrue(productDetailsViewModel.HasBeenOrdered == true);
            ordersResult = await shoppingService.GetCustomerOrders(new GetUserOrdersArgs { CustomerId = customerId });
            Assert.IsTrue(ordersResult.Status.Succeeded);
            Assert.IsTrue(ordersResult.List.Count > 0);
            Assert.IsTrue(ordersResult.List.ContainsProduct(product.Id));
        }

        static void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INavigationService, MockNavigationService>();
            serviceCollection.AddSingleton<IShoppingService, MockShoppingService>();
            serviceCollection.AddSingleton<ITaxService, TaxService>();
            serviceCollection.AddSingleton<ITaxCalculatorService, TaxJarCalculatorService>();

            serviceCollection.AddTransient<ShopPageViewModel>();
            serviceCollection.AddSingleton<ProductDetailViewModel>();

            m_serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
