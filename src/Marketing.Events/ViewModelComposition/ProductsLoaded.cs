namespace Marketing.Events.ViewModelComposition
{
    using System.Collections.Generic;

    public class ProductsLoaded
    {
        public IDictionary<int, dynamic> AvailableProductsViewModel { get; set; }
    }
}