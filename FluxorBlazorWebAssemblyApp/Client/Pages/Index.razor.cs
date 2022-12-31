using Fluxor;
using FluxorBlazorWebAssemblyApp.Client.Store;
using Microsoft.AspNetCore.Components;

namespace FluxorBlazorWebAssemblyApp.Client.Pages;

public partial class Index
{
    [Inject] public required IState<CountState> State { get; init; }
    [Inject] public required IState<CountState2> State2 { get; init; }
    [Inject] public required IDispatcher Dispatcher { get; init; }
    [Inject] public required IActionSubscriber ActionSubscriber { get; init; }

    protected override void OnInitialized()
    {
        State.StateChanged += (_,__) =>
        {
            Dispatcher.Dispatch(new PostIncrementCountAction());
            StateHasChanged();
        };
        State2.StateChanged += (_, __) =>
        {
            StateHasChanged();
        };
        ActionSubscriber.SubscribeToAction<IncrementCountAction>(this, (action) =>
        {
            Console.WriteLine("123");
            //Dispatcher.Dispatch(new PostIncrementCountAction());
        });
        base.OnInitialized();
    }



    public void Add()
    {
        Dispatcher.Dispatch(new IncrementCountAction());
    }
}