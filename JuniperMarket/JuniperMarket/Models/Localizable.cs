using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Models
{
    public class Localizable
    {
        public const string ShopPageTitle = "Shop at Juniper";
        public const string ProductDetailsPageTitle = "Product Details";
        public const string ShopPageLoadingMessage = "Getting your shopping list...";
        public const string ProductDetailsLoadingMessage = "Getting your product...";

        public const string PlaceOrderSuccessTitle = "Congratulations!";
        public const string PlaceOrderSuccessMessage = "We've successfully placed your order!";
        public const string ShopEmptyExperienceMainMessage = "It looks like there aren't any products available right now.";
        public const string ShopEmptyExperienceSecondaryMessage = "Please try us again later - we're always on the lookout for new products to stock!";

        public const string ShopErrorExperienceMainMessage = "We had a bit of trouble retrieving our current list of in-stock products.";
        public const string ShopErrorExperienceSecondaryMessage = "Rest assured that we're working on it - please try again soon!";

        public const string OrderedProductMessageTemplate = "You ordered this product on {0}, for {1} after tax and shipping.";
        public const string ErrorCalculatingTaxForOrderMessage = "Whoops! We had trouble with that calculation - please try again.";
        public const string ErrorPlacingProductOrder = "We had trouble placing your order - please try again.";
    }
}
