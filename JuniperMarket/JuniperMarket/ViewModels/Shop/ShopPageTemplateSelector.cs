using System;
using Xamarin.Forms;

namespace JuniperMarket.ViewModels.Shop
{
    public class ShopPageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LoadingTemplate { get; set; }

        public DataTemplate ProductCategoryTemplate { get; set; }

        public DataTemplate ProductTemplate { get; set; }

        public DataTemplate EmptyExperienceTemplate { get; set; }

        public DataTemplate ErrorExperienceTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ShopLoadingCellViewModel)
            {
                return LoadingTemplate;
            }
            else if (item is ShopProductCategoryCellViewModel)
            {
                return ProductCategoryTemplate;
            }
            else if (item is ShopProductCellViewModel)
            {
                return ProductTemplate;
            }
            else if (item is ShopEmptyExperienceCellViewModel)
            {
                return EmptyExperienceTemplate;
            }
            else if (item is ShopErrorExperienceCellViewModel)
            {
                return ErrorExperienceTemplate;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
