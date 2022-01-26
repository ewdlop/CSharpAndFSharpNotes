using ConsoleApp1.Portraits.Common;
using ConsoleApp1.Portraits.Interfaces;
using ConsoleApp1.Portraits.Services;
using ConsoleApp1.Portraits.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.AddMemoryCache();
    services.AddPortraitImageProvider(context.Configuration, (options) =>
    {
        //the options can overwrite the configuration option form the appsettings.json(s)
        //for the ones that are supplied here
    });
});

IHost host = builder.Build();

using (IServiceScope services = host.Services.CreateScope())
{
    IServiceProvider serviceProvider = services.ServiceProvider;
    IImageProvider imageProvider = serviceProvider.GetRequiredService<IImageProvider>();

    Random random = new Random();
    string[] names = { "MA","MS","JD","HF", "OK","NJ","NY","PA" };
    
    await Parallel.ForEachAsync(Enumerable.Range(0,100), async (dmcid,token) => {
        //int chosenIndex = dmcid % imageProvider.NumberOfImagesForMale;
        //int chosenIndex = dmcid % imageProvider.NumberOfImagesForFemale;
        int chosenIndex = dmcid % imageProvider.NumberOfImagesForAllGender;
        await imageProvider.GeneratePortraitBasedOnGenderAsync(names[RandomNumberGenerator.GetRandomNumber(0, names.Length)],
                                                               "PartyProfileFullGenerator",
                                                               fileName: dmcid.ToString(),
                                                               chosenIndex: chosenIndex,
                                                               PortraitImageProviderOptions.Gender.DEFAULT);
        Console.Write("#");
    });
}