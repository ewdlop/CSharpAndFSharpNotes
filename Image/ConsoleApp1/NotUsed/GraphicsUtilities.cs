using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DM2BD.Europa.DAL.Generators
{
    public static class GraphicsUtilities
    {
        public static class Font
        {
            public static readonly string Tahoma = "Tahoma";
        }

        public static void CreateNewImageWithText(string text,
                                          string font,
                                          float emsize,
                                          string inputImageFilePath,
                                          string outputImageFilePath,
                                          System.Drawing.Imaging.ImageFormat imageFormat)
        {
            using(Image outputImage = Image.FromFile(inputImageFilePath))
            {
                // Create a rectangle for the entire bitmap 
                RectangleF rectangleFloat = new(0, 0, outputImage.Width, outputImage.Height);
                // Create graphic object that will draw onto the bitmap 
                using (Graphics drawingContext = Graphics.FromImage(outputImage))
                {
                    // ------------------------------------------ 
                    // Ensure the best possible quality rendering 
                    // ------------------------------------------ 
                    // The smoothing mode specifies whether lines, curves,
                    // and the edges of filled areas use smoothing (also called antialiasing).  
                    // One exception is that path gradient brushes do not obey the smoothing mode.  
                    // Areas filled using a PathGradientBrush are rendered the same way (aliased)
                    // regardless of the SmoothingMode property. 

                    drawingContext.SmoothingMode = SmoothingMode.AntiAlias;

                    // The interpolation mode determines how intermediate values between two endpoints are calculated. 
                    drawingContext.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object. 
                    drawingContext.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    // This one is important 
                    drawingContext.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    // Create string formatting options (used for alignment) 
                    StringFormat stringFomrat = new()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    // Draw the text onto the image 
                    drawingContext.DrawString(text,
                                              new System.Drawing.Font(font, 24),
                                              Brushes.White,
                                              rectangleFloat,
                                              stringFomrat);
                    //Flush all graphics changes to the bitmap
                    drawingContext.Flush();
                }
                outputImage.Save(outputImageFilePath, imageFormat);
            }
        }

        public static byte[] CreateImageWithText(string text,
                                  string font,
                                  float emsize,
                                  string inputImageFilePath,
                                  System.Drawing.Imaging.ImageFormat imageFormat)
        {
            using (MemoryStream memoryStream = new())
            {
                using (Bitmap outputImage = new(inputImageFilePath))
                {
                    // Create a rectangle for the entire bitmap 
                    RectangleF rectangleFloat = new(0, 0, outputImage.Width, outputImage.Height);
                    // Create graphic object that will draw onto the bitmap 
                    using (Graphics drawingContext = Graphics.FromImage(outputImage))
                    {
                        // ------------------------------------------ 
                        // Ensure the best possible quality rendering 
                        // ------------------------------------------ 
                        // The smoothing mode specifies whether lines, curves,
                        // and the edges of filled areas use smoothing (also called antialiasing).  
                        // One exception is that path gradient brushes do not obey the smoothing mode.  
                        // Areas filled using a PathGradientBrush are rendered the same way (aliased)
                        // regardless of the SmoothingMode property. 

                        drawingContext.SmoothingMode = SmoothingMode.AntiAlias;

                        // The interpolation mode determines how intermediate values between two endpoints are calculated. 
                        drawingContext.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        // Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object. 
                        drawingContext.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        // This one is important 
                        drawingContext.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        // Create string formatting options (used for alignment) 
                        StringFormat stringFomrat = new()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        // Draw the text onto the image 
                        drawingContext.DrawString(text,
                                                  new System.Drawing.Font(font, 24),
                                                  Brushes.White,
                                                  rectangleFloat,
                                                  stringFomrat);
                        //Flush all graphics changes to the bitmap
                        drawingContext.Flush();
                    }
                    outputImage.Save(memoryStream, imageFormat);
                }
                return memoryStream.ToArray();
            }
        }
    }
}
