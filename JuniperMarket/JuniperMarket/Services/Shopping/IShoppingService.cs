using JuniperMarket.Models.Shopping;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Shopping
{
    public interface IShoppingService
    {
        /// <summary>
        /// For demo purposes, there will always be a valid signed-in user.
        /// </summary>
        Customer CurrentSignedInCustomer { get; }

        /// <summary>
        /// Returns a list of all available (i.e. 'in stock') products, capped at maxCount. If maxCount
        /// is 0, all available products will be returned.
        /// </summary>
        Task<ServiceOperationResult<Product>> GetAvailableProducts(int maxCount = 50);

        /// <summary>
        /// Returns the specified product.
        /// </summary>
        /// <param name="args">args.ProductId: the id of the product to be returned.</param>
        /// <returns>
        /// <para>ServiceResultCode.FailedInvalidArgs: args.ProductId is null.</para>
        /// <para>ServiceResultCode.FailedObjectNotFound: the Product couldn't be found.</para>
        /// </returns>
        Task<ServiceOperationResult<Product>> GetProduct(GetProductArgs args);

        /// <summary>
        /// Attempts to place an order for a product, on behalf of the specified customer.
        /// </summary>
        /// <param name="args">
        ///     args.ProductId: the product to be ordered. 
        ///     args.CustomerId: the customer placing the order.
        /// </param>
        /// <returns>
        /// <para>ServiceResultCode.FailedInvalidArgs: args.ProductId or args.CustomerId is null.</para>
        /// <para>ServiceResultCode.FailedNotAuthorized: the customer specified by CustomerId isn't signed in.</para>
        /// <para>ServiceResultCode.FailedObjectNotFound: the Product couldn't be found.</para>
        /// </returns>
        Task<ServiceOperationResult<Order>> OrderProduct(OrderProductArgs args);

        /// <summary>
        /// Returns all the orders for the specified customer.
        /// </summary>
        /// <param name="args">args.CustomerId</param>
        /// <returns>
        ///     <para>ServiceResultCode.FailedInvalidArgs: args.CustomerId is null.</para>
        ///     <para>ServiceResultCode.FailedNotAuthorized: the specified customer isn't signed in.</para>
        /// </returns>
        Task<ServiceOperationResult<Order>> GetCustomerOrders(GetUserOrdersArgs args);

        /// <summary>
        /// Returns the Order representing the order placed by the given customer of the given product.
        /// </summary>
        /// <returns>
        ///     <para>ServiceResultCode.FailedInvalidArgs: customerId or productId is null.</para>
        ///     <para>ServiceResultCode.FailedNotAuthorized: the specified customer isn't signed in.</para>
        /// </returns>
        Task<ServiceOperationResult<Order>> GetOrderedProductForCustomer(string customerId, string productId);

        /// <summary>
        /// For mocking and development purposes, enables injection of the given errorCode for all API calls, until DisableErrorResult is called.
        /// </summary>
        void InjectErrorResult(ServiceResultCode errorCode);

        /// <summary>
        /// Disables the injection of API error results.
        /// </summary>
        void DisableErrorResult();
    }
}
