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
using BlazorServerApp.Products;
using BlazorServerApp.Data;

namespace BlazorServerApp.Pages
{
    public partial class FetchData
    {
        [Inject]
        private WeatherForecastService WeatherForecastService { get; init; }
        private AddProductToCartResponse _addProductToCartResponse;
        private WeatherForecast[] _forecasts;
        protected override async Task OnInitializedAsync()
        {
            _forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
            await base.OnInitializedAsync();
        }
    }
}