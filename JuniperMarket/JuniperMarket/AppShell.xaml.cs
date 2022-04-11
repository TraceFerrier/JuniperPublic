using JuniperMarket.ViewModels;
using JuniperMarket.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace JuniperMarket
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
        }

    }
}
