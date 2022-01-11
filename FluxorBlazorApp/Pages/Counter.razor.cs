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
using FluxorBlazorApp.Store;
using Fluxor;

namespace FluxorBlazorApp.Pages
{

    public partial class Counter
    {
        [Inject]
        private IState<CounterState> CounterState { get; set; }
        [Inject]
        public IDispatcher Dispatcher { get; set; }
        private void IncrementCount()
        {
            var action = new IncrementCounterAction();
            Dispatcher.Dispatch(action);
        }
    }
}