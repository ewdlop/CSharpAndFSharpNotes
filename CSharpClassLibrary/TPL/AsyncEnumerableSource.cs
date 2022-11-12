using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace CSharpClassLibrary.DataFlow;

public class AsyncEnumerableSource<T> : IAsyncEnumerable<T>
{
    private readonly List<Channel<T>> _channels = new();
    private bool _completed;
    private Exception _exception;

    public async IAsyncEnumerable<T> GetAsyncEnumerable(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Channel<T> channel;
        lock (_channels)
        {
            if (_exception != null) throw _exception;
            if (_completed) yield break;
            channel = Channel.CreateUnbounded<T>(
                new() { SingleWriter = true, SingleReader = true });
            _channels.Add(channel);
        }
        try
        {
            await foreach (var item in channel.Reader.ReadAllAsync(cancellationToken)
                .WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return item;
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
        finally { lock (_channels) _channels.Remove(channel); }
    }

    public void YieldReturn(T value)
    {
        lock (_channels)
        {
            if (_completed) return;
            foreach (var channel in _channels) channel.Writer.TryWrite(value);
        }
    }

    public void Complete()
    {
        lock (_channels)
        {
            if (_completed) return;
            foreach (var channel in _channels) channel.Writer.TryComplete();
            _completed = true;
        }
    }

    public void Fault(Exception error)
    {
        lock (_channels)
        {
            if (_completed) return;
            foreach (var channel in _channels) channel.Writer.TryComplete(error);
            _completed = true;
            _exception = error;
        }
    }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return GetAsyncEnumerable(cancellationToken).GetAsyncEnumerator();
    }
}