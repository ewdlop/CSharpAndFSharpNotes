namespace ConsoleApp1.Portraits.Interfaces;

public interface IImage
{
    ReadOnlySpan<char> Name { get; }

    ReadOnlySpan<byte> Data { get; }

    ReadOnlySpan<char> Class { get; }
}
