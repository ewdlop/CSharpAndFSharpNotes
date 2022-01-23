namespace DM2BD.Europa.DAL.Generators.Portraits.Interfaces;

public interface IProvideIndividualImages
{
    Task<byte[]> GetPortraitImageBytesBasedOnGenderAsync(string text, string customGroupName, int? chosenIndex = null, string? gender = null, CancellationToken cancellationToken = default);
    Task GeneratePortraitBasedOnGenderAsync(string text, string customGroupName, string? fileName = null, int? chosenIndex = null, string? gender = null, string? outputPathPrefix = null, CancellationToken cancellationToken = default);
    int NumberOfImagesForMale { get; }
    int NumberOfImagesForFemale { get; }
    int NumberOfImagesForAllGender { get; }
}