using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Services.Shopping
{
    public class OrderProductArgs : BaseShoppingArgs
    {
        public string ProductId { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
