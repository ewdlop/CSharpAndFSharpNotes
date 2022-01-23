namespace DM2BD.Europa.DAL.Generators.Portraits.Interfaces;

public interface IImage
{
    ReadOnlySpan<char> Name { get; }

    ReadOnlySpan<byte> Data { get; }

    ReadOnlySpan<char> Class { get; }
}
