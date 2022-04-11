using JuniperMarket.ViewModels;
using JuniperMarket.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JuniperMarket.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(ShellNavigationState state)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync(state);
            }
        }

        public async Task PushModalAsync(Page page)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PushModalAsync(page);
            }
        }

        public async Task<Page> PopModalAsync()
        {
            if (Shell.Current != null)
            {
                return await Shell.Current.Navigation.PopModalAsync();
            }

            return null;
        }

        public async Task PushModelAsync<TPage>(BaseViewModel viewModel) where TPage : BaseContentPage, new()
        {
            var page = new TPage();
            page.BindViewModel(viewModel);
            await PushModalAsync(page);
        }

        public async Task ShowMessage(string title, string message)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert(title, message, "Ok");
            }
        }
    }
}
