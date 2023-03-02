namespace BlazorApp3.Pages;

public partial class Index
{
    public int x { get; set; }
    protected override void OnInitialized()
    {
        Console.WriteLine($"=========================");
        Console.WriteLine($"OnInitialized:{Environment.CurrentManagedThreadId}");
        base.OnInitialized();
    }

    //protected override Task OnInitializedAsync()
    //{
    //    //    Console.WriteLine($"OnInitializedAsync:{Environment.CurrentManagedThreadId}");
    //    //    await Task.Delay(3000);
    //    //    Console.WriteLine($"OnInitializedAsync after delay:{Environment.CurrentManagedThreadId}");
    //    return Task.CompletedTask;
    //}

    //protected override async Task OnInitializedAsync()
    //{
    //    Console.WriteLine($"OnInitializedAsync:{Environment.CurrentManagedThreadId}");
    //    await Task.Delay(3000);
    //    Console.WriteLine($"OnInitializedAsync after delay:{Environment.CurrentManagedThreadId}");
    //}

    protected override void OnParametersSet()
    {
        Console.WriteLine($"OnParametersSet:{Environment.CurrentManagedThreadId}");
        base.OnParametersSet();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if(firstRender)
        {
            Console.WriteLine($"First OnAfterRender:{Environment.CurrentManagedThreadId}");
        }
        Console.WriteLine(x);
        Console.WriteLine($"OnAfterRender:{Environment.CurrentManagedThreadId}");
        Console.WriteLine($"=========================");
        base.OnAfterRender(firstRender);
    }

    public async void OnClick()
    {
        Console.WriteLine($"OnClick:{Environment.CurrentManagedThreadId}");
        await Task.Delay(1000);
        Console.WriteLine($"OnClick After delay:{Environment.CurrentManagedThreadId}");
        _ = InvokeAsync(() =>
        {
            Console.WriteLine($"OnClick InvokeAsync:{Environment.CurrentManagedThreadId}");
            x = Environment.CurrentManagedThreadId;
            StateHasChanged();
        });
    }
}
