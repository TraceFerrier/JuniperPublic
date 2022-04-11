using JuniperMarket.Models.Shopping;
using JuniperMarket.Extensions;
using System.Windows.Input;
using Xamarin.Forms;
using JuniperMarket.Views;
using JuniperMarket.Services;

namespace JuniperMarket.ViewModels.Shop
{
    public class ShopProductCellViewModel : BaseShopCellViewModel
    {
        private readonly INavigationService m_navigationService;

        public ShopProductCellViewModel(Product product, INavigationService navigationService)
        {
            Product = product;
            m_navigationService = navigationService;
            ProductTapCommand = new Command(OnProductTapCommand);
        }

        public Product Product { get; set; }

        public ICommand ProductTapCommand { get; set; }

        private async void OnProductTapCommand()
        {
            await m_navigationService.GoToAsync($"{nameof(ProductDetailPage)}?{nameof(ProductDetailViewModel.ProductId)}={Product.Id}");
        }

        public string FormattedPrice
        {
            get
            {
                return Product.Price.FormatAsCurrency();
            }
        }
    }
}
