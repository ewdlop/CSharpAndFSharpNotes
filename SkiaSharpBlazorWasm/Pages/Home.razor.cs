using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using SkiaSharp.Views.Blazor;

namespace SkiaSharpBlazorWasm.Pages;

public partial class Home
{
    [Inject] public required IJSRuntime JS { get; set; }
    protected SKCanvasView? _skiaView;
    public SKPoint? TouchLocation { get; protected set; }

    void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        // the the canvas and properties
        var canvas = e.Surface.Canvas;

        // make sure the canvas is blank
        canvas.Clear(SKColors.White);

        // decide what the text looks like
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Center
        };
        using var font = new SKFont
        {
            Size = 24,
        };

        // adjust the location based on the pointer
        var coord = (TouchLocation is SKPoint loc)
            ? new SKPoint(loc.X, loc.Y)
            : new SKPoint(e.Info.Width / 2, (e.Info.Height + font.Size) / 2);

        // draw some text
        canvas.DrawText("SkiaSharp", coord.X, coord.Y, font, paint);
    }

    void OnPointerDown(PointerEventArgs e)
    {
        TouchLocation = new SKPoint((float)e.OffsetX, (float)e.OffsetY);
        _skiaView?.Invalidate();
    }

    void OnPointerMove(PointerEventArgs e)
    {
        if (TouchLocation is null)
            return;

        TouchLocation = new SKPoint((float)e.OffsetX, (float)e.OffsetY);
        _skiaView?.Invalidate();
    }

    void OnPointerUp(PointerEventArgs e)
    {
        TouchLocation = null;
        _skiaView?.Invalidate();
    }
}