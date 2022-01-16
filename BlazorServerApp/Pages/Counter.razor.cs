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
using BlazorServerApp.Colors;

namespace BlazorServerApp.Pages
{
    public partial class Counter : IDisposable
    {
        private int currentCount = 0;
        private string color;
        protected override void OnInitialized()
        {
            ColorChanged += OnColorChanged;
            base.OnInitialized();
        }
        private void IncrementCount()
        {
            currentCount++;
        }
        protected async void OnColorChanged(object obj, UpdateColorEventArgs args)
        {
            color = args.Color;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            ColorChanged -= OnColorChanged;
        }
    }
}