using JuniperMarket.Services;
using JuniperMarket.ViewModels;
using JuniperMarket.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JuniperMarketTest.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public Task GoToAsync(ShellNavigationState state)
        {
            return Task.CompletedTask;
        }

        public Task<Page> PopModalAsync()
        {
            return Task.FromResult(new Page());
        }

        public Task PushModalAsync(Page page)
        {
            return Task.CompletedTask;
        }

        public Task PushModelAsync<TPage>(BaseViewModel viewModel) where TPage : BaseContentPage, new()
        {
            return Task.CompletedTask;
        }

        public Task ShowMessage(string title, string message)
        {
            return Task.CompletedTask;
        }
    }
}
