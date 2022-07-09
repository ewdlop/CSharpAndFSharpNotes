using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Pages;

public partial class Child : ComponentBase
{
    [Parameter]
    public int TestField { get; set; }


    [Parameter]
    public int TestProperty { get; set; }

    protected override void OnInitialized()
    {
        Console.WriteLine("Child OnInitialized");
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("Child OnInitializedAsync");
        await base.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        Console.WriteLine("Child OnParametersSet");
        base.OnParametersSet();
    }

    protected override Task OnParametersSetAsync()
    {
        Console.WriteLine("Child OnParametersSetAsync");
        return base.OnParametersSetAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine("Child OnAfterRender");
        base.OnAfterRender(firstRender);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        Console.WriteLine("Child OnAfterRenderAsync");
        return base.OnAfterRenderAsync(firstRender);
    }
}