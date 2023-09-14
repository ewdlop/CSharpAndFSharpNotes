using BlazorApp3.Data;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Pages;

public partial class Counter : FluxorComponent
{
    [Inject]
    public required IState<TestState<int>> CounterState { get; set; }
    [Inject]
    public required IState<ConcreteTestState> ConcreteTestState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    //private void IncrementCount()
    //{
    //    Console.WriteLine($"=========================");
    //    Console.WriteLine($"IncrementCount:{Environment.CurrentManagedThreadId}");
    //    var action = new IncrementCounterAction();
    //    Dispatcher.Dispatch(action);
    //}

    private void IncrementCount()
    {
        Console.WriteLine($"=========================");
        Console.WriteLine($"IncrementCount:{Environment.CurrentManagedThreadId}");
        var action = new CreateAction<int>(x => new ConcreteTestState(x.ClickCount+1));
        Dispatcher.Dispatch(action);
        Dispatcher.Dispatch(new IncrementCounterAction());
    }

}