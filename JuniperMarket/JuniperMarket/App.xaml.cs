using JuniperMarket.Services;
using JuniperMarket.Services.Navigation;
using JuniperMarket.Services.Shopping;
using JuniperMarket.Services.Tax;
using JuniperMarket.ViewModels;
using JuniperMarket.ViewModels.Profile;
using JuniperMarket.ViewModels.Shop;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;

namespace JuniperMarket
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            RegisterDependencies();
            MainPage = new AppShell();
        }

        public static BaseViewModel GetViewModel<T>() where T : BaseViewModel
        {
            return ServiceProvider.GetService<T>();
        }

        void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INavigationService, NavigationService>();
            serviceCollection.AddSingleton<IShoppingService, MockShoppingService>();
            serviceCollection.AddSingleton<ITaxService, TaxService>();

            // In the future, we can register additional tax calculator implementations here
            // as needed, and the TaxService can select the most appropriate among them based
            // on current customer, ordered product, etc.
            serviceCollection.AddSingleton<ITaxCalculatorService, TaxJarCalculatorService>();

            serviceCollection.AddTransient<ShopPageViewModel>();
            serviceCollection.AddTransient<ProductDetailViewModel>();
            serviceCollection.AddTransient<ProfilePageViewModel>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static IServiceProvider ServiceProvider { get; set; }
    }
}
