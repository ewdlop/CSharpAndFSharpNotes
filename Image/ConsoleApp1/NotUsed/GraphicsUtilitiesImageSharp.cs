using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace DM2BD.Europa.DAL.Generators
{
    public enum ImageFormat
    {
        JPG,
        PNG
    }
    public static class GraphicsUtilitiesImageSharp
    {
        public static async Task CreateNewImageWithText(string text,
                                          string font,
                                          float fontSize,
                                          string inputImageFilePath,
                                          string outputImageFilePath,
                                          CancellationToken cancellationToken = default)
        {
            using (var image = await Image.LoadAsync(inputImageFilePath, cancellationToken))
            {
                //FontFamily fontFamily = new FontFamily();
                //Font font = new Font();
                PointF point = new PointF(image.Width / 2, image.Height / 2);
                //image.Mutate(i => i.DrawText(text, font, Color.Black, point));
                await image.SaveAsync(outputImageFilePath, cancellationToken);
            }
        }
    }
}

