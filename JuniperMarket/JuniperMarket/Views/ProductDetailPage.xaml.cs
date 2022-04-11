using JuniperMarket.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JuniperMarket.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : BaseContentPage
    {
        public ProductDetailPage()
        {
            InitializeComponent();
            BindViewModel<ProductDetailViewModel>();
        }
    }
}