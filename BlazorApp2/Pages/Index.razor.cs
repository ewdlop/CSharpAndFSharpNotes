using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp2.Pages;

public partial class Index
{
    [Inject] public required IJSRuntime JSRuntime { get; init; }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("reactRender");
    }
}