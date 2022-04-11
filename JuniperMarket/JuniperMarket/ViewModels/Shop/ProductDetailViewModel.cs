using JuniperMarket.Extensions;
using JuniperMarket.Models;
using JuniperMarket.Models.Shopping;
using JuniperMarket.Services;
using JuniperMarket.Services.Shopping;
using JuniperMarket.Services.Tax;
using JuniperMarket.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JuniperMarket.ViewModels.Shop
{
    [QueryProperty(nameof(ProductId), nameof(ProductId))]
    public class ProductDetailViewModel : BaseViewModel
    {
        public ProductDetailViewModel(IShoppingService shoppingService, ITaxService taxService, INavigationService navigationService)
        {
            m_shoppingService = shoppingService;
            m_taxService = taxService;
            m_navigationService = navigationService;
            PageTitle = Localizable.ProductDetailsPageTitle;
            CalculateCommand = new Command(OnCalculateCommand);
            BuyNowCommand = new Command(OnBuyNowCommand);
            ConfirmBuyCommand = new Command(OnConfirmBuyCommand);
            CancelBuyCommand = new Command(OnCancelBuyCommand);
            IsAvailableToBuy = true;
        }

        public Customer CurrentCustomer
        {
            get { return m_shoppingService.CurrentSignedInCustomer; }
        }

        public Command CalculateCommand { get; set; }

        public Command BuyNowCommand { get; set; }

        public Command ConfirmBuyCommand { get; set; }

        public Command CancelBuyCommand { get; set; }

        public bool IsTaxCalculated
        {
            get { return m_isTaxCalculated; }
            set { SetProperty(ref m_isTaxCalculated, value); }
        }

        public bool IsActivityRunning
        {
            get { return m_isActivityRunning; }
            set { SetProperty(ref m_isActivityRunning, value); }
        }

        public bool IsAvailableToBuy
        {
            get { return m_isAvailableToBuy; }
            set { SetProperty(ref m_isAvailableToBuy, value); }
        }

        public bool HasBeenOrdered
        {
            get { return m_hasBeenOrdered; }
            set 
            {
                SetProperty(ref m_hasBeenOrdered, value);
                IsAvailableToBuy = !m_hasBeenOrdered;
                NotifyPropertyChanged(() => HasBeenOrderedMessage);
            }
        }

        public string HasBeenOrderedMessage
        {
            get
            {
                if (HasBeenOrdered && OrderForProduct != null)
                {
                    var finalPrice = OrderForProduct.FinalPrice.FormatAsCurrency(); 
                    return string.Format(Localizable.OrderedProductMessageTemplate, OrderForProduct.OrderDate, finalPrice);
                }

                return null;
            }
        }

        public bool ShouldShowStatusMessage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(m_statusMessage);
            }
        }

        public string StatusMessage
        {
            get { return m_statusMessage; }
            set
            {
                SetProperty(ref m_statusMessage, value);
                NotifyPropertyChanged(() => ShouldShowStatusMessage);
            }
        }

        private async void OnCalculateCommand()
        {
            await CalculateTaxAndShipping();
        }

        private async Task<bool> CalculateTaxAndShipping()
        {
            try
            {
                StatusMessage = null;
                IsActivityRunning = true;
                var customer = CurrentCustomer;
                var salesTaxResult = await m_taxService.GetSalesTaxForOrder(customer, Product);
                if (salesTaxResult.Status.Succeeded)
                {
                    var taxInfo = salesTaxResult.Persistable;
                    ShippingCost = (decimal)taxInfo.Shipping;
                    TaxesCost = (decimal)taxInfo.AmountToCollect;
                    IsTaxCalculated = true;
                    return true;
                }
                else
                {
                    StatusMessage = Localizable.ErrorCalculatingTaxForOrderMessage;
                    return false;
                }
            }
            finally
            {
                IsActivityRunning = false;
            }
        }

        public async Task StartBuyProduct()
        {
            if (!IsTaxCalculated)
            {
                bool isCalculated = await CalculateTaxAndShipping();
                if (!isCalculated)
                {
                    return;
                }
            }

            await m_navigationService.PushModelAsync<BuyProductPage>(this);
        }

        private async void OnBuyNowCommand()
        {
            await StartBuyProduct();
        }

        private async void OnConfirmBuyCommand()
        {
            await FinishBuyProduct();
        }

        public async Task FinishBuyProduct()
        {
            var orderResult = await m_shoppingService.OrderProduct(new OrderProductArgs 
            { 
                CustomerId = CurrentCustomer.Id, 
                ProductId = Product.Id, 
                FinalPrice = GrandTotal
            });

            if (orderResult.Status.Succeeded)
            {
                await m_navigationService.ShowMessage(Localizable.PlaceOrderSuccessTitle, Localizable.PlaceOrderSuccessMessage);
                OrderForProduct = orderResult.Persistable;
                HasBeenOrdered = true;
            }
            else
            {
                StatusMessage = Localizable.ErrorPlacingProductOrder;
            }

            await m_navigationService.PopModalAsync();
        }

        private async void OnCancelBuyCommand()
        {
            await m_navigationService.PopModalAsync();
        }

        public string PageTitle
        {
            get { return m_pageTitle; }
            set { SetProperty(ref m_pageTitle, value); }
        }

        public string LoadingMessage
        {
            get { return Localizable.ProductDetailsLoadingMessage; }
        }

        public string ProductName
        {
            get { return m_productName; }
            set { SetProperty(ref m_productName, value); }
        }

        public string ProductDescription
        {
            get { return m_productDescription; }
            set { SetProperty(ref m_productDescription, value); }
        }

        public string ProductImageUrl
        {
            get { return m_productImageUrl; }
            set { SetProperty(ref m_productImageUrl, value); }
        }

        public decimal ProductPrice
        {
            get
            {
                return m_productPrice;
            }

            set
            {
                SetProperty(ref m_productPrice, value);
                NotifyPropertyChanged(() => FormattedPrice);
                UpdateGrandTotal();
            }
        }
        public string FormattedPrice
        {
            get
            {
                return ProductPrice.FormatAsCurrency();
            }
        }

        public decimal ShippingCost
        {
            get
            {
                return m_shippingCost;
            }

            set
            {
                SetProperty(ref m_shippingCost, value);
                NotifyPropertyChanged(() => FormattedShippingCost);
                UpdateGrandTotal();
            }
        }

        public string FormattedShippingCost
        {
            get
            {
                return ShippingCost.FormatAsCurrency();
            }
        }


        public decimal TaxesCost
        {
            get
            {
                return m_taxesCost;
            }

            set
            {
                SetProperty(ref m_taxesCost, value);
                NotifyPropertyChanged(() => FormattedTaxesCost);
                UpdateGrandTotal();
            }
        }

        public string FormattedTaxesCost
        {
            get
            {
                return TaxesCost.FormatAsCurrency();
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return ProductPrice + ShippingCost + TaxesCost;
            }
        }

        public string FormattedGrandTotal
        {
            get
            {
                return GrandTotal.FormatAsCurrency();
            }
        }

        public void UpdateGrandTotal()
        {
            NotifyPropertyChanged(() => GrandTotal);
            NotifyPropertyChanged(() => FormattedGrandTotal);
        }

        public int ProductQuantityAvailable
        {
            get { return m_productQuantityAvailable; }
            set { SetProperty(ref m_productQuantityAvailable, value); }
        }

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public Order OrderForProduct { get; set; }

        public override async Task OnViewAppearing(bool isFirstAppearance)
        {
            if (!isFirstAppearance)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(ProductId))
            {
                var productResult = await m_shoppingService.GetProduct(new GetProductArgs { ProductId = ProductId });
                if (productResult.Status.Succeeded)
                {
                    Product = productResult.Persistable;
                    var ordersResult = await m_shoppingService.GetOrderedProductForCustomer(CurrentCustomer.Id, ProductId);
                    if (ordersResult.Status.Succeeded)
                    {
                        OrderForProduct = ordersResult.Persistable;
                        HasBeenOrdered = true;
                    }

                    UpdateViewModel(Product);
                }
            }
        }

        private void UpdateViewModel(Product product)
        {
            ProductName = product.Name;
            ProductDescription = product.Description;
            ProductImageUrl = product.ProductImageUrl;
            ProductPrice = product.Price;
            ProductQuantityAvailable = product.QuantityAvailable;
        }

        private readonly IShoppingService m_shoppingService;
        private readonly ITaxService m_taxService;
        private readonly INavigationService m_navigationService;
        private string m_pageTitle;
        private string m_productName;
        private string m_productDescription;
        private string m_productImageUrl;
        private decimal m_productPrice;
        private decimal m_shippingCost;
        private decimal m_taxesCost;
        private bool m_isActivityRunning;
        private bool m_isAvailableToBuy;
        private bool m_hasBeenOrdered;
        private bool m_isTaxCalculated;
        private bool m_shouldShowStatusMessage;
        private string m_statusMessage;
        private int m_productQuantityAvailable;
    }
}
