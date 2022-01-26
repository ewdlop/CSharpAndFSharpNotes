namespace ConsoleApp1.Portraits.Options;

public partial class PortraitImageProviderOptions
{
    public const string CONFIGURE_SECTION_NAME = "PortraitImageOptions";
    /// <summary>
    /// In pixels
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// In pixels
    /// </summary>
    public int Height { get; set; }
    /// <summary>
    /// Use 0.5 for 50%
    /// </summary>
    public float PercentangeOfWidth { get; set; }
    public int FontSize { get; set; }
    /// <summary>
    /// Arial, Calibri, or Tahoma, etc
    /// </summary>
    public string FontFamily { get; set; }
    /// <summary>
    /// Not configurable yet. Currently the default is .png
    /// </summary>
    public string OutputFormat { get; } = ".png";
    public string RawImagesPathPrefix { get; set; }
    public string OutputPathPrefix { get; set; }
    public string[] ClassIDsPathPrefix { get; set; } = new[] { INDIVIDUAL };
}
