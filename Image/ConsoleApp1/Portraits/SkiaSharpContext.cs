using SkiaSharp;

namespace DM2BD.Europa.DAL.Generators.Portraits;

public static class SkiaSharpContext
{
    public const string DEFAULT_TEXT = "Lorem Ipsum";
    public const string DEFAULT_FONT_FAMILY = "Arial";
    public static byte[] GenerateTextedImageBytesAsync(byte[] imageBytes,
                                                       string text = DEFAULT_TEXT,
                                                       string fontFamily = DEFAULT_FONT_FAMILY,
                                                       float percentangefWidth = 1.0f,
                                                       int width = 100,
                                                       int height = 100,
                                                       int quality = 100)
    {
        using SKBitmap bitmap = SKBitmap.Decode(imageBytes);
        using SKCanvas drawingCanvas = new(bitmap);
        using (SKPaint textPaint = new()
        {
            IsAntialias = true,
            Color = SKColors.White,
            Typeface = SKTypeface.FromFamilyName(fontFamily),
            //TextAlign = SKTextAlign.Center //not work as intended
        })
        {
            float textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = percentangefWidth * width * textPaint.TextSize / textWidth;
            // Find the text bounds
            SKRect textBounds = new();
            textPaint.MeasureText(text, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = width / 2 - textBounds.MidX;
            float yText = height / 2 - textBounds.MidY;

            SKPoint center = new(xText, yText);
            drawingCanvas.DrawText(text, center, textPaint);
            drawingCanvas.Flush();
            using SKImage image = SKImage.FromBitmap(bitmap);
            using (SKData data = image.Encode(SKEncodedImageFormat.Png, quality))
            {
                return data.ToArray();
            }
        }
    }
    public static string[] GetSupportedFontFamilies()
    {
        using (SKFontManager skFontManager = SKFontManager.CreateDefault())
        {
            return skFontManager.GetFontFamilies();
        }
    }

    public static string[] GetSupportedImageFormat() => Enum.GetNames(typeof(SKEncodedImageFormat));
}
