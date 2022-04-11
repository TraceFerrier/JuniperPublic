using JuniperMarket.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace JuniperMarket.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
        }

        public void BindViewModel<T>() where T : BaseViewModel
        {
            ViewModel = App.GetViewModel<T>();
            ViewModel.IsLoading = true;
            ViewModel.IsLoaded = false; 
            BindingContext = ViewModel;
        }

        public void BindViewModel(BaseViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.IsLoading = true;
            ViewModel.IsLoaded = false;
            BindingContext = ViewModel;
        }

        public BaseViewModel ViewModel { get; private set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel != null)
            {
                await ViewModel.OnViewAppearing(m_isFirstAppearance);
                if (m_isFirstAppearance)
                {
                    m_isFirstAppearance = false;
                }
                ViewModel.IsLoading = false;
                ViewModel.IsLoaded = true;
            }
        }

        private bool m_isFirstAppearance = true;
    }
}