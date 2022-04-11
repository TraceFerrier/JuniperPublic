using JuniperMarket.Models;
using JuniperMarket.Models.Shopping;
using JuniperMarket.Services;
using JuniperMarket.Services.Shopping;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JuniperMarket.ViewModels.Shop
{
    public class ShopPageViewModel : BaseViewModel
    {
        public ShopPageViewModel(IShoppingService shoppingService, INavigationService navigationService)
        {
            m_shoppingService = shoppingService;
            m_navigationService = navigationService;
            ShoppingListViewModels = new ObservableCollection<BaseShopCellViewModel>();
            ShoppingListViewModels.Add(new ShopLoadingCellViewModel { LoadingMessage = Localizable.ShopPageLoadingMessage });

            RefreshPageCommand = new Command(OnRefreshPageCommand);
            PageTitle = Localizable.ShopPageTitle;
        }

        public string PageTitle { get; set; }

        public ObservableCollection<BaseShopCellViewModel> ShoppingListViewModels { get; private set; }

        public ICommand RefreshPageCommand { get; set; }

        public bool IsPageRefreshing
        {
            get { return m_isPageRefreshing; }
            set { SetProperty(ref m_isPageRefreshing, value); }
        }

        private async void OnRefreshPageCommand()
        {
            IsPageRefreshing = true;
            await RetrieveProducts();
            IsPageRefreshing = false;
        }

        public override async Task OnViewAppearing(bool isFirstAppearance)
        {
            if (isFirstAppearance)
            {
                await RetrieveProducts();
            }
        }

        private async Task RetrieveProducts()
        {
            ServiceOperationResult<Product> productsResult = await m_shoppingService.GetAvailableProducts(maxCount: 75);
            BuildProductList(productsResult);
        }

        private void BuildProductList(ServiceOperationResult<Product> productsResult)
        {
            ShoppingListViewModels.Clear();
            if (productsResult != null && productsResult.Status.Succeeded)
            {
                if (productsResult.List.Count > 0)
                {
                    foreach (var product in productsResult.List)
                    {
                        var productCellItem = new ShopProductCellViewModel(product, m_navigationService);
                        ShoppingListViewModels.Add(productCellItem);
                    }
                }
                else
                {
                    ShoppingListViewModels.Add(new ShopEmptyExperienceCellViewModel 
                    {
                        MainMessage = Localizable.ShopEmptyExperienceMainMessage,
                        SecondaryMessage = Localizable.ShopEmptyExperienceSecondaryMessage,
                    });
                }
            }
            else
            {
                ShoppingListViewModels.Add(new ShopErrorExperienceCellViewModel
                {
                    MainMessage = Localizable.ShopErrorExperienceMainMessage,
                    SecondaryMessage = Localizable.ShopErrorExperienceSecondaryMessage,
                });
            }
        }

        private readonly IShoppingService m_shoppingService;
        private readonly INavigationService m_navigationService;
        private bool m_isPageRefreshing;
    }
}
