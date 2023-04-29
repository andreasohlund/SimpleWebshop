namespace Marketing.ViewModelComposition
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Marketing.Events.ViewModelComposition;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ServiceComposer.AspNetCore;
    using ITOps.ViewModelComposition;

    internal class ProductsListGetHandler : ICompositionRequestsHandler
    {
        readonly HttpClient httpClient;

        public ProductsListGetHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [HttpGet("/products")]
        public async Task Handle(HttpRequest request)
        {
            var url = $"/api/available/products";
            var response = await httpClient.GetAsync(url);

            var availableProducts = await response.Content.As<int[]>();
            var availableProductsViewModel = MapToDictionary(availableProducts);

            var context = request.GetCompositionContext();
            var vm = request.GetComposedResponseModel();
            await context.RaiseEvent(new ProductsLoaded
            {
                AvailableProductsViewModel = availableProductsViewModel
            });

            vm.AvailableProducts = availableProductsViewModel.Values.ToList();
            ////invoke Marketing back-end API to retrieve the current products
            //var url = $"http://localhost:50688/product/";
            //var client = new HttpClient();
            //var response = await client.GetAsync(url);
            //dynamic products = await response.Content.AsExpandoArrayAsync();

            //// Create a dictionary that's keyed by OrderId. 
            //var orderDictionary = MapToViewModelDictionary(products);

            //// Raise an event so that other views that need t
            //// enrich the view with more data related to each OrderId .  
            //await vm.RaiseEventAsync(new ProductsLoaded {OrdersDictionary = orderDictionary});

            //// Store the enriched data in the viewmodel.
            //vm.Products = orderDictionary.Values;
        }

        IDictionary<int, dynamic> MapToDictionary(IEnumerable<int> availableProducts)
        {
            var availableProductsViewModel = new Dictionary<int, dynamic>();

            foreach (var id in availableProducts)
            {
                dynamic vm = new ExpandoObject();
                vm.Id = id;

                availableProductsViewModel[id] = vm;
            }

            return availableProductsViewModel;
        }

        //IDictionary<dynamic, dynamic> MapToViewModelDictionary(dynamic[] products)
        //{
        //    var dictionary = new Dictionary<dynamic, dynamic>();

        //    foreach (var product in products)
        //    {
        //        dynamic productDetailObject = new ExpandoObject();
        //        productDetailObject.productId = product.productId;
        //        productDetailObject.name = product.name;
        //        productDetailObject.imageUrl = product.imageUrl;
        //        dictionary[product.productId] = productDetailObject;
        //    }

        //    return dictionary;
        //}
    }
}