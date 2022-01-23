using DM2BD.Europa.DAL.Generators.Portraits.Interfaces;
using DM2BD.Europa.DAL.Generators.Portraits.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DM2BD.Europa.DAL.Generators.Portraits;

public partial class PortraitImageProvider : IImageProvider
{
    private record Image(string Name, byte[] Data, string Class) : IImage
    {
        ReadOnlySpan<char> IImage.Name => Name.AsSpan();

        ReadOnlySpan<byte> IImage.Data => Data.AsSpan();
        ReadOnlySpan<char> IImage.Class => Class.AsSpan();
    }

    private readonly PortraitImageProviderOptions _options;
    //needs to inject IMemoryCache behind a IRepository and use Mediator to access it
    //so this service doesn't always directly on IMemoryCache or something
    private readonly IMemoryCache _memoryCache;
    public PortraitImageProvider(IMemoryCache memoryCache,
                                 IOptions<PortraitImageProviderOptions> options)
    {
        _memoryCache = memoryCache;
        _options = options.Value;
        _maleIndividualImageNamesArray = _options.IndividualImageNames.Males.ToImmutableArray();
        _femaleIndividualImageNamesArray = _options.IndividualImageNames.Females.ToImmutableArray();
        _allIndividualImageNamesArray = _maleIndividualImageNamesArray.Union(_femaleIndividualImageNamesArray).ToImmutableArray();
    }
    
}
