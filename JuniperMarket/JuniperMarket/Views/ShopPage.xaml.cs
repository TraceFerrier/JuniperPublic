using JuniperMarket.ViewModels.Shop;
using Xamarin.Forms.Xaml;

namespace JuniperMarket.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopPage : BaseContentPage
    {
        public ShopPage()
        {
            InitializeComponent();
            BindViewModel<ShopPageViewModel>();
        }
    }
}