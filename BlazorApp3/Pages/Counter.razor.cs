using BlazorApp3.Data;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Pages;

public partial class Counter : FluxorComponent
{
    [Inject]
    private IState<CounterState> CounterState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    private void IncrementCount()
    {
        Console.WriteLine($"=========================");
        Console.WriteLine($"IncrementCount:{Environment.CurrentManagedThreadId}");
        var action = new IncrementCounterAction();
        Dispatcher.Dispatch(action);
    }

}