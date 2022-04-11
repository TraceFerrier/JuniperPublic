using JuniperMarket.Models.Shopping;
using JuniperMarket.Services.Shopping;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.ViewModels.Profile
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel(IShoppingService shoppingService)
        {
            m_shoppingService = shoppingService;
            UpdateViewModel(m_shoppingService.CurrentSignedInCustomer);
        }

        public string FullName
        {
            get { return m_fullName; }
            set { SetProperty(ref m_fullName, value); }
        }

        public string ProfileImageUrl
        {
            get { return m_profileImageUrl; }
            set { SetProperty(ref m_profileImageUrl, value); }
        }

        public string Location
        {
            get { return m_location; }
            set { SetProperty(ref m_location, value); }
        }

        public string Bio
        {
            get { return m_bio; }
            set { SetProperty(ref m_bio, value); }
        }

        private void UpdateViewModel(Customer customer)
        {
            ProfileImageUrl = customer.ProfileImageUrl;
            FullName = customer.FullName;
            Location = string.Format("{0}, {1}", customer.HomeAddress.City, customer.HomeAddress.State);
            Bio = customer.Bio;
        }

        private readonly IShoppingService m_shoppingService;
        private string m_profileImageUrl;
        private string m_location;
        private string m_fullName;
        private string m_bio;

    }
}
