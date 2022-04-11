using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Models.Shopping
{
    public class BaseShoppingEntity
    {
        public BaseShoppingEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
