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

    //public async void OnClick()
    //{
    //    Console.WriteLine($"OnClick:{Environment.CurrentManagedThreadId}");
    //    await Task.Delay(1000);
    //    Console.WriteLine($"OnClick After delay:{Environment.CurrentManagedThreadId}");
    //    _ = InvokeAsync(async () =>
    //    {
    //        await Task.Delay(2000);
    //        Console.WriteLine($"OnClick InvokeAsync:{Environment.CurrentManagedThreadId}");
    //        x = Environment.CurrentManagedThreadId;
    //        StateHasChanged();
    //    });
    //    Console.WriteLine($"OnClick Fire-And-Forget delay:{Environment.CurrentManagedThreadId}");
    //}

    //public void OnClick()
    //{
    //    Console.WriteLine(SynchronizationContext.Current?.GetHashCode());
    //    Console.WriteLine($"OnClick:{Environment.CurrentManagedThreadId}");
    //    x = Environment.CurrentManagedThreadId;
    //    StateHasChanged();
    //}

    public async void OnClick()
    {
        Console.WriteLine(SynchronizationContext.Current?.GetHashCode());
        Console.WriteLine($"OnClick:{Environment.CurrentManagedThreadId}");

        Parallel.ForEach(Enumerable.Range(0, 10000), async i =>
        {
            await Task.Delay(1000);
            Console.WriteLine(SynchronizationContext.Current?.GetHashCode());
            Console.WriteLine($"OnClick:{Environment.CurrentManagedThreadId}");
            x = i;
            StateHasChanged();
        });
    }

    //public void OnClick()
    //{
    //    Console.WriteLine($"OnClick:{Environment.CurrentManagedThreadId}");
    //    //Parallel.ForEachAsync(Enumerable.Range(0, 1000), async (i, c) =>
    //    //{
    //    //    Console.WriteLine($"ForEachAsync i :{Environment.CurrentManagedThreadId}");
    //    //    await Task.Delay(1000, c);
    //    //    Console.WriteLine($"ForEachAsync i After delay:{Environment.CurrentManagedThreadId}");
    //    //    _ = InvokeAsync(() =>
    //    //    {
    //    //        Console.WriteLine($"OnClick InvokeAsync:{Environment.CurrentManagedThreadId}");
    //    //        x = Environment.CurrentManagedThreadId;
    //    //        StateHasChanged();
    //    //    });
    //    //});

    //    Parallel.ForEach(Enumerable.Range(0, 1000),  new ParallelOptions()
    //    {
    //        MaxDegreeOfParallelism = 100
    //    },i =>
    //    {
    //        Console.WriteLine($"ForEach i :{Environment.CurrentManagedThreadId}");
    //        InvokeAsync(() =>
    //        {
    //            Console.WriteLine($"ForEach i InvokeAsync:{Environment.CurrentManagedThreadId}");
    //            x = Environment.CurrentManagedThreadId;
    //            StateHasChanged();
    //        });
    //    });
    //}
}
