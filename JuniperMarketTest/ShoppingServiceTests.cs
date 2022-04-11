using System;
using JuniperMarket.Services.Shopping;
using JuniperMarket.Services.Tax;
using JuniperMarket.ViewModels.Shop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using JuniperMarket.Services;

namespace JuniperMarketTest
{
    [TestClass]
    public class ShoppingServiceTests : BaseTestClass
    {
        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            RegisterDependencies();
        }

        [TestMethod]
        public async Task ShoppingServiceShouldReturnNoMoreThanSpecifiedProductCount()
        {
            var shoppingService = GetService<IShoppingService>();
            var productsResult = await shoppingService.GetAvailableProducts(maxCount: 3);
            Assert.IsNotNull(productsResult);
            Assert.IsTrue(productsResult.Status.Succeeded);

            var products = productsResult.List;
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count >0 && products.Count <= 3);
        }

        [TestMethod]
        public async Task ShouldRejectOrderFromNotSignedInCustomer()
        {
            var shoppingService = GetService<IShoppingService>();
            var productsResult = await shoppingService.OrderProduct(new OrderProductArgs
            {
                CustomerId=Guid.NewGuid().ToString(),
                ProductId=Guid.NewGuid().ToString(),
            });
            Assert.IsNotNull(productsResult);
            Assert.IsTrue(productsResult.Status.ResultCode == ServiceResultCode.FailedNotAuthorized);
        }

        [TestMethod]
        public async Task ShouldRejectGetOrdersRequestFromNotSignedInCustomer()
        {
            var shoppingService = GetService<IShoppingService>();
            var productsResult = await shoppingService.GetCustomerOrders(new GetUserOrdersArgs
            {
                CustomerId = Guid.NewGuid().ToString(),
            });
            Assert.IsNotNull(productsResult);
            Assert.IsTrue(productsResult.Status.ResultCode == ServiceResultCode.FailedNotAuthorized);
        }

        [TestMethod]
        public async Task ShouldRejectGetOrderedProductRequestFromNotSignedInCustomer()
        {
            var shoppingService = GetService<IShoppingService>();
            var productResult = await shoppingService.GetOrderedProductForCustomer(customerId: Guid.NewGuid().ToString(), productId: Guid.NewGuid().ToString());
            Assert.IsNotNull(productResult);
            Assert.IsTrue(productResult.Status.ResultCode == ServiceResultCode.FailedNotAuthorized);
        }

        [TestMethod]
        public async Task ShouldRejectGetOrderedProductRequestWithInvalidProductId()
        {
            var shoppingService = GetService<IShoppingService>();
            var productResult = await shoppingService.GetOrderedProductForCustomer(shoppingService.CurrentSignedInCustomer.Id, productId: Guid.NewGuid().ToString());
            Assert.IsNotNull(productResult);
            Assert.IsTrue(productResult.Status.ResultCode == ServiceResultCode.FailedObjectNotFound);
        }

        [TestMethod]
        public async Task ShouldRejectGetProductWithInvalidProductId()
        {
            var shoppingService = GetService<IShoppingService>();
            var productsResult = await shoppingService.GetProduct(new GetProductArgs
            {
                ProductId=Guid.NewGuid().ToString(),
            });
            Assert.IsNotNull(productsResult);
            Assert.IsTrue(productsResult.Status.ResultCode == ServiceResultCode.FailedObjectNotFound);

            productsResult = await shoppingService.GetProduct(new GetProductArgs());
            Assert.IsNotNull(productsResult);
            Assert.IsTrue(productsResult.Status.ResultCode == ServiceResultCode.FailedInvalidArgs);
        }


        static void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IShoppingService, MockShoppingService>();
            serviceCollection.AddSingleton<ITaxService, TaxService>();
            serviceCollection.AddSingleton<ITaxCalculatorService, TaxCalculatorService>();

            m_serviceProvider = serviceCollection.BuildServiceProvider();
        }

    }
}