using JuniperMarket.Extensions;
using JuniperMarket.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniperMarket.Services.Shopping
{
    public class MockShoppingService : IShoppingService
    {
        const int m_normalSimulatedDelayInMs = 600;
        const int m_shortSimulatedDelayInMs = 300;
        const int m_minProducts = 15;
        const int m_maxProducts = 75;

        // For demo UI purposes, let's ship from/to the same state, so that taxes are always applicable.
        SupportedUSAStates m_shippingState = ShoppingFactory.GetRandomState();

        public MockShoppingService()
        {
            CurrentSignedInCustomer = ShoppingFactory.GenerateCustomer(m_shippingState);
            m_injectedErrorCode = ServiceResultCode.Ok;
        }

        public Customer CurrentSignedInCustomer { get; }

        public async Task<ServiceOperationResult<Product>> GetAvailableProducts(int maxCount)
        {
            if (m_injectedErrorCode != ServiceResultCode.Ok)
            {
                return new ServiceOperationResult<Product>(m_injectedErrorCode);
            }

            if (m_availableProducts.Count == 0)
            {
                PopulateProducts();
            }

            List<Product> products;
            if (maxCount > 0 && maxCount < m_availableProducts.Count)
            {
                products = m_availableProducts.ToList().GetRange(0, maxCount);
            }
            else
            {
                products = m_availableProducts.ToList();
            }

            var result = new ServiceOperationResult<Product>(products);
            await Task.Delay(m_normalSimulatedDelayInMs);
            return result;
        }

        public async Task<ServiceOperationResult<Product>> GetProduct(GetProductArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.ProductId))
            {
                return new ServiceOperationResult<Product>(ServiceResultCode.FailedInvalidArgs);
            }

            if (m_availableProducts.TryGetValue(args.ProductId, out Product product))
            {
                await Task.Delay(m_normalSimulatedDelayInMs);
                return new ServiceOperationResult<Product>(product);
            }

            return new ServiceOperationResult<Product>(ServiceResultCode.FailedObjectNotFound);

        }

        public async Task<ServiceOperationResult<Order>> GetCustomerOrders(GetUserOrdersArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.CustomerId))
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedInvalidArgs);
            }

            if (args.CustomerId != CurrentSignedInCustomer.Id)
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedNotAuthorized);
            }

            if (m_orders.TryGetValue(args.CustomerId, out var customerOrders))
            {
                await Task.Delay(m_normalSimulatedDelayInMs);
                return new ServiceOperationResult<Order>(customerOrders);
            }

            return new ServiceOperationResult<Order>(new List<Order>());

        }

        public async Task<ServiceOperationResult<Order>> GetOrderedProductForCustomer(string customerId, string productId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(productId))
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedInvalidArgs);
            }

            if (customerId != CurrentSignedInCustomer.Id)
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedNotAuthorized);
            }

            await Task.Delay(m_shortSimulatedDelayInMs);
            if (m_orders.TryGetValue(customerId, out var customerOrders))
            {
                foreach (var order in customerOrders)
                {
                    if (order.ProductId == productId)
                    {
                        return new ServiceOperationResult<Order>(order);
                    }
                }
            }

            return new ServiceOperationResult<Order>(ServiceResultCode.FailedObjectNotFound);

        }

        public async Task<ServiceOperationResult<Order>> OrderProduct(OrderProductArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.CustomerId) || string.IsNullOrWhiteSpace(args.ProductId))
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedInvalidArgs);
            }

            if (args.CustomerId != CurrentSignedInCustomer.Id)
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedNotAuthorized);
            }

            if (!m_availableProducts.ContainsKey(args.ProductId))
            {
                return new ServiceOperationResult<Order>(ServiceResultCode.FailedObjectNotFound);
            }

            var order = new Order
            {
                CustomerId = args.CustomerId,
                ProductId = args.ProductId,
                OrderDate = DateTime.Now,
                FinalPrice = args.FinalPrice,
                Quantity = 1
            };

            if (!m_orders.ContainsKey(order.CustomerId))
            {
                m_orders.Add(order.CustomerId, new List<Order>());
            }
            m_orders[order.CustomerId].Add(order);
            await Task.Delay(m_shortSimulatedDelayInMs);

            return new ServiceOperationResult<Order>(order);

        }

        public void InjectErrorResult(ServiceResultCode errorCode)
        {
            m_injectedErrorCode = errorCode;
        }

        public void DisableErrorResult()
        {
            m_injectedErrorCode = ServiceResultCode.Ok;
        }

        private void PopulateProducts()
        {
            m_availableProducts.Clear();

            var products = ShoppingFactory.GenerateProducts(m_minProducts, m_maxProducts, m_shippingState);
            foreach (var product in products)
            {
                m_availableProducts.Add(product.Id, product);
            }
        }

        private Dictionary<string, Product> m_availableProducts=new Dictionary<string, Product>();
        private Dictionary<string, List<Order>> m_orders = new Dictionary<string, List<Order>>();
        private ServiceResultCode m_injectedErrorCode;
    }
}
