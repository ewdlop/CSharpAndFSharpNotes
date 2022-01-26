using ConsoleApp1.Portraits;
using ConsoleApp1.Portraits.Common;
using ConsoleApp1.Portraits.Interfaces;
using ConsoleApp1.Portraits.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.Portraits;

public partial class PortraitImageProvider : IProvideIndividualImages
{
    private readonly ImmutableArray<string> _maleIndividualImageNamesArray;
    private readonly ImmutableArray<string> _femaleIndividualImageNamesArray;
    private readonly ImmutableArray<string> _allIndividualImageNamesArray;
    public int NumberOfImagesForMale => _maleIndividualImageNamesArray.Length;
    public int NumberOfImagesForFemale => _femaleIndividualImageNamesArray.Length;
    public int NumberOfImagesForAllGender => _allIndividualImageNamesArray.Length;
    private async ValueTask<byte[]> GetRandomPortraitBytesBasedOnGenderAsync(string customGroupName,
                                                                             string? gender = null,
                                                                             CancellationToken cancellationToken = default)
    {

        (int min, int max) = gender?.ToLowerInvariant() switch
        {
            PortraitImageProviderOptions.Gender.FEMALE => (0, NumberOfImagesForFemale),
            PortraitImageProviderOptions.Gender.MALE => (0, NumberOfImagesForMale),
            _ => (0, NumberOfImagesForAllGender)
        };
        int randomNumber = RandomNumberGenerator.GetRandomNumber(min, max);
        (int imageNumber, string customGroup) extendedKey = (randomNumber, $"{customGroupName}-Individual-{gender}-random");
        Image image = _memoryCache.Get<Image>(extendedKey);
        if (image == null)
        {
            string randomChosenImageName = gender?.ToLowerInvariant() switch
            {
                PortraitImageProviderOptions.Gender.FEMALE => _femaleIndividualImageNamesArray[randomNumber],
                PortraitImageProviderOptions.Gender.MALE => _maleIndividualImageNamesArray[randomNumber],
                _ => _allIndividualImageNamesArray[randomNumber]
            };
            string rawImagePath = Path.Combine(_options.RawImagesPathPrefix, randomChosenImageName);
            byte[] imageRawBytes = await File.ReadAllBytesAsync(rawImagePath, cancellationToken);
            image = new Image(randomChosenImageName, imageRawBytes, "Individual");
            _memoryCache.Set(extendedKey, image);
        }
        return image.Data;
    }

    private async ValueTask<byte[]> GetPortraitBytesBasedOnGenderAsync(int chosenIndex,
                                                                       string customGroupName,
                                                                       string? gender = null,
                                                                       CancellationToken cancellationToken = default)
    {
        //IMemoryCache is injected and shared through out the app, so we needs a bigger key to uniquely indentify the image we want
        (int imageNumber, string customGroup) extendedKey = (chosenIndex, $"{customGroupName}-Individual-{gender}");
        //Image image = await _memoryCache.GetOrCreateAsync(extendedKey, async (entry) =>
        //{
        //    string chosenImageName = gender?.ToLowerInvariant() switch
        //    {
        //        PortraitImageProviderOptions.Gender.FEMALE => _femaleIndividualImageNamesArray[Math.Max(0, Math.Min(chosenIndex, NumberOfImagesForFemale - 1))],
        //        PortraitImageProviderOptions.Gender.MALE => _maleIndividualImageNamesArray[Math.Max(0, Math.Min(chosenIndex, NumberOfImagesForMale - 1))],
        //        _ => _allIndividualImageNamesArray[Math.Max(0, Math.Min(chosenIndex, NumberOfImagesForAllGender - 1))]
        //    };
        //    string rawImagePath = Path.Combine(_options.RawImagesPathPrefix, chosenImageName);
        //    byte[] imageRawBytes = await File.ReadAllBytesAsync(rawImagePath, cancellationToken);
        //    return new Image(chosenImageName, imageRawBytes, "Individual");
        //});

        Image image2 = _memoryCache.Get<Image>(extendedKey);
        if (image2 == null)
        {
            string chosenImageName = gender?.ToLowerInvariant() switch
            {
                PortraitImageProviderOptions.Gender.FEMALE => _femaleIndividualImageNamesArray[Math.Max(0, Math.Min(chosenIndex, NumberOfImagesForFemale - 1))],
                PortraitImageProviderOptions.Gender.MALE => _maleIndividualImageNamesArray[Math.Max(0, Math.Min(chosenIndex, NumberOfImagesForMale - 1))],
                _ => _allIndividualImageNamesArray[Math.Max(0, Math.Min(chosenIndex, NumberOfImagesForAllGender - 1))]
            };
            string rawImagePath = Path.Combine(_options.RawImagesPathPrefix, chosenImageName);
            byte[] imageRawBytes = await File.ReadAllBytesAsync(rawImagePath, cancellationToken);
            image2 = _memoryCache.Set(extendedKey, new Image(chosenImageName, imageRawBytes, "Individual"));
        }

        return image2.Data;
    }
    public async Task<byte[]> GetPortraitImageBytesBasedOnGenderAsync(string text,
                                                                      string customGroupName,
                                                                      int? chosenIndex = null,
                                                                      string? gender = null,
                                                                      CancellationToken cancellationToken = default)
    {
        byte[] imageBytes = chosenIndex switch
        {
            not null => await GetPortraitBytesBasedOnGenderAsync(chosenIndex.Value, customGroupName, gender, cancellationToken),
            _ => await GetRandomPortraitBytesBasedOnGenderAsync(customGroupName, gender, cancellationToken),
        };
        return SkiaSharpContext.GenerateTextedImageBytesAsync(imageBytes,
                                                              text,
                                                              _options.FontFamily,
                                                              _options.PercentangeOfWidth,
                                                              _options.Width,
                                                              _options.Height);
    }

    public async Task GeneratePortraitBasedOnGenderAsync(string text,
                                                         string customGroupName,
                                                         string? fileName = null,
                                                         int? chosenIndex = null,
                                                         string? gender = null,
                                                         string? outputPathPrefix = null,
                                                         CancellationToken cancellationToken = default)
    {
        if (outputPathPrefix == null)
        {
            outputPathPrefix = _options.OutputPathPrefix;
        }
        if (fileName == null)
        {
            fileName = Guid.NewGuid().ToString();
        }
        byte[] data = await GetPortraitImageBytesBasedOnGenderAsync(text, customGroupName, chosenIndex, gender, cancellationToken);
        //string outFilePath = Path.Combine(outputPathPrefix, $"{fileName}-{Environment.CurrentManagedThreadId}{_options.OutputFormat}");
        string outFilePath = Path.Combine(outputPathPrefix, $"{fileName}-{_options.OutputFormat}");
        await File.WriteAllBytesAsync(outFilePath, data, cancellationToken);
    }
}