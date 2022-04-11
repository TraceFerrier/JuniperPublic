using JuniperMarket.Models.Shopping;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace JuniperMarket.Extensions
{
    public static class Extensions
    {
        public static string FormatAsCurrency(this decimal val)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            var formattedPrice = val.ToString("C", numberFormatInfo);
            return formattedPrice;
        }

        public static List<T> ToList<T>(this Dictionary<string, T> dict)
        {
            var list = new List<T>();
            foreach (var kvp in dict)
            {
                list.Add(kvp.Value);
            }

            return list;
        }

        public static string EndWithPeriod(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (!str.EndsWith("."))
            {
                str = str + ".";
            }

            return str;
        }

        /// <summary>
        /// Returns true if the given list of orders contains an order for the product with the given id.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool ContainsProduct(this List<Order> list, string productId)
        {
            foreach (var order in list)
            {
                if (order.ProductId == productId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
