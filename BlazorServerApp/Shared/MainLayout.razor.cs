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

namespace BlazorServerApp.Shared
{
    public partial class MainLayout : IDisposable
    {
        private EventHandler<UpdateColorEventArgs> eventHandler;
        protected override void OnInitialized()
        {
            eventHandler = async (sender, eventArgs) => { await OnColorChanged(sender, eventArgs); };
            ColorChanged += eventHandler;
            base.OnInitialized();
        }

        public void Dispose()
        {
            ColorChanged -= eventHandler;
        }
    }
}