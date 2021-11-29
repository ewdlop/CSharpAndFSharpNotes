using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using BlazorWebAssemblyApp.Client;
using BlazorWebAssemblyApp.Client.Shared;
using SkiaSharp;
using SkiaSharp.Views.Blazor;

namespace BlazorWebAssemblyApp.Client.Pages
{
    public partial class SkiaCanvasItem
    {
        SKCanvasView skiaView = null !;
        SKPoint? touchLocation;
        //[Inject] IJSRuntime JS { get; set; } = null!;
        void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            // the the canvas and properties
            var canvas = e.Surface.Canvas;
            // make sure the canvas is blank
            canvas.Clear(SKColors.White);
            // decide what the text looks like
            using var paint = new SKPaint{Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Center, TextSize = 24};
            // adjust the location based on the pointer
            var coord = (touchLocation is SKPoint loc) ? new SKPoint(loc.X, loc.Y) : new SKPoint(e.Info.Width / 2, (e.Info.Height + paint.TextSize) / 2);
            // draw some text
            canvas.DrawText("SkiaSharp", coord, paint);
        }

        void OnPointerDown(PointerEventArgs e)
        {
            touchLocation = new SKPoint((float)e.OffsetX, (float)e.OffsetY);
            skiaView.Invalidate();
        }

        void OnPointerMove(PointerEventArgs e)
        {
            if (touchLocation == null)
                return;
            touchLocation = new SKPoint((float)e.OffsetX, (float)e.OffsetY);
            skiaView.Invalidate();
        }

        void OnPointerUp(PointerEventArgs e)
        {
            touchLocation = null;
            skiaView.Invalidate();
        }
    }
}