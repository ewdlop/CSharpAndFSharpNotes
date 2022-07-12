namespace BlazorApp1.Pages;

public partial class FetchData
{
    public int count = 0;
    public int CountProperty = 0;
    public int parentCount = 0;
    private int i;

    private void StateHasChangedTest()
    {
        Console.WriteLine("==================StateHasChanged==================");
        StateHasChanged();
    }
    private async Task StateHasChangedTestAsync()
    {
        Console.WriteLine("==================StateHasChangedAsync==================");
        await InvokeAsync(StateHasChanged);
    }

    private void UpateParentCount()
    {
        Console.WriteLine("==================UpateParentCount==================");
        parentCount++;
    }
    
    private async Task UpateParentCountAsync()
    {
        Console.WriteLine("==================UpateParentCountAsync==================");
        Console.WriteLine($"ParentCount:{parentCount}");
        await Task.Delay(1);
        parentCount++;
    }

    private async Task UpateCountAsync()
    {
        Console.WriteLine("==================UpateCount==================");
        await Task.Delay(1);
        count++;
    }
    private void UpateCount()
    {
        Console.WriteLine("==================UpateCount==================");
        count++;
    }

    private void UpateCountProperty()
    {
        Console.WriteLine("==================UpateCountProperty==================");
        CountProperty++;
    }

    private async Task UpateCountPropertyAsync()
    {
        Console.WriteLine("==================UpateCountProperty==================");
        await Task.Delay(1);
        CountProperty++;
    }

    protected override void OnInitialized()
    {
        i++;
        Console.WriteLine("====");
        Console.WriteLine("Parent OnInitialized");
        Console.WriteLine($"i:{i}");
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("Parent OnInitializedAsync");
        await base.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        Console.WriteLine("Parent OnParametersSet");
        base.OnParametersSet();
    }

    protected override Task OnParametersSetAsync()
    {
        Console.WriteLine("Parent OnParametersSetAsync");
        return base.OnParametersSetAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine("Parent OnAfterRender");
        if(firstRender)
        {
            //parentCount++;
            //CountProperty++;
            //count++;
            //StateHasChanged();
        }
        Console.WriteLine($"ParentCount:{parentCount}");
        base.OnAfterRender(firstRender);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        Console.WriteLine("Parent OnAfterRenderAsync");
        return base.OnAfterRenderAsync(firstRender);
    }
}