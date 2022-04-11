using JuniperMarket.ViewModels.Common;
using System.Threading.Tasks;

namespace JuniperMarket.ViewModels
{
    public class BaseViewModel : BasePropertyChanged
    {
        public virtual async Task OnViewAppearing(bool isFirstAppearance)
        {
            await Task.CompletedTask;
        }

        public bool IsLoading
        {
            get { return m_isLoading; }
            set { SetProperty(ref m_isLoading, value); }
        }

        public bool IsLoaded
        {
            get { return m_isLoaded; }
            set { SetProperty(ref m_isLoaded, value); }
        }

        private bool m_isLoading;
        private bool m_isLoaded;

        string m_title = string.Empty;
        public string Title
        {
            get { return m_title; }
            set { SetProperty(ref m_title, value); }
        }

    }
}
