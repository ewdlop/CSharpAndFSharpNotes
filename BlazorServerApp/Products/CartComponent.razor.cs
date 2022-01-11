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
using BlazorServerApp;
using BlazorServerApp.Shared;
using MediatR;

namespace BlazorServerApp.Products;

public partial class CartComponent
{
    private List<Product> _products;
    protected override async Task OnInitializedAsync()
    {
        _products = new List<Product>();
        ProductAdded += AddProduct;
    }

    public void Dispose()
    {
        ProductAdded -= AddProduct;
    }

    public async void AddProduct(object sender, AddProductToBasketEventArgs args)
    {
        _products.Add(args.Product);
        await InvokeAsync(StateHasChanged);
    }
}
