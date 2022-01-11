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
using FluxorBlazorApp;
using FluxorBlazorApp.Shared;
using Fluxor;
using FluxorBlazorApp.Data;
using FluxorBlazorApp.Store;

namespace FluxorBlazorApp.Pages;

public partial class FetchData
{
    [Inject]
    private IState<WeatherState> WeatherState { get; set; }
    [Inject]
    private IDispatcher Dispatcher { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(new FetchDataAction());
    }
}
