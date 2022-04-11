using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Models.Shopping
{
    public class Product : BaseShoppingEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string ProductImageUrl { get; set; }

        public decimal Price { get; set; }

        public decimal ShippingCost { get; set; }

        public int QuantityAvailable { get; set; }

        public Address ShipsFromAddress { get; set; }
    }
}
