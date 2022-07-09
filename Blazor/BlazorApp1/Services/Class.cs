using Microsoft.JSInterop;

namespace BlazorApp1.Services
{
    public sealed class ClipboardService
    {
        private readonly IJSRuntime _jsRuntime;

        public ClipboardService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<ReadOnlyMemory<char>> ReadTextAsync()
        {
            return _jsRuntime.InvokeAsync<ReadOnlyMemory<char>>("navigator.clipboard.readText");
        }

        public ValueTask WriteTextAsync(ReadOnlyMemory<char> text)
        {
            return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
    }
}