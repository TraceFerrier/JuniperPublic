using JuniperMarket.ViewModels;
using JuniperMarket.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JuniperMarket.Services
{
    public interface INavigationService
    {
        Task GoToAsync(ShellNavigationState state);

        Task PushModalAsync(Page page);

        Task PushModelAsync<TPage>(BaseViewModel viewModel) where TPage : BaseContentPage, new();

        Task<Page> PopModalAsync();

        Task ShowMessage(string title, string message);
    }
}
