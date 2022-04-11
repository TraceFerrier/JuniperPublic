using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Models.Shopping
{
    public class Order : BaseShoppingEntity
    {
        public string CustomerId { get; set; }

        public string ProductId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal FinalPrice { get; set; }

        public int Quantity { get; set; }

    }
}
