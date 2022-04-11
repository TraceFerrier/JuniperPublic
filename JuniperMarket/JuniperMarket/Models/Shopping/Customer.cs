using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Models.Shopping
{
    public class Customer : BaseShoppingEntity
    {
        public Customer()
        {
            ShippingAddress = new Address();
        }

        public string FullName { get; set; }

        public string ProfileImageUrl { get; set; }

        public Address HomeAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public string Bio { get; set; }
    }
}
