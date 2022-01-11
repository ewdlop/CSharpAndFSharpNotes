using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorServerApp.Shared;
using MediatR;
using BlazorServerApp.Pages;

namespace BlazorServerApp.Products
{
    public partial class CartComponent : IRequestHandler<AddProductToCartCommand, AddProductToCartResponse>
    {
        public static event EventHandler<AddProductToBasketEventArgs> ProductAdded;
        private static List<AddProductToCartCommand> _addProductToCartCommands = new List<AddProductToCartCommand>();
        public async Task<AddProductToCartResponse> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            decimal totalPrice = _addProductToCartCommands.Sum(x => x.Product.Price) + request.Product.Price;
            if (totalPrice < 200)
            {
                ProductAdded?.Invoke(this, new AddProductToBasketEventArgs { Product = request.Product });
                _addProductToCartCommands.Add(request);
                return new AddProductToCartResponse { Succeeded = true };
            }
            return new AddProductToCartResponse
            {
                Succeeded = false,
                Message = $"{totalPrice} exceeds the limit of 200$"
            };
        }
    }
}