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
            //TODO: hide this
            var url = "http://localhost:50688/product";
            var response = await httpClient.GetAsync(url);

            var availableProducts = await response.Content.AsExpandoArray();
            var availableProductsViewModel = MapToViewModelDictionary(availableProducts);

            var context = request.GetCompositionContext();
            var vm = request.GetComposedResponseModel();
            await context.RaiseEvent(new ProductsLoaded
            {
                AvailableProductsViewModel = availableProductsViewModel
            });

            vm.Products = availableProductsViewModel.Values.ToList();
        }

        IDictionary<dynamic, dynamic> MapToViewModelDictionary(dynamic[] products)
        {
            var dictionary = new Dictionary<dynamic, dynamic>();

            //TODO: sync with mauro
            foreach (var product in products)
            {
                dynamic productDetailObject = new ExpandoObject();
                productDetailObject.ProductId = product.ProductId;
                productDetailObject.Name = product.Name;
                productDetailObject.ImageUrl = product.ImageUrl;
                
                //TODO:
                productDetailObject.InStock = true;
                dictionary[product.ProductId] = productDetailObject;
            }
            return dictionary;
        }
    }
}