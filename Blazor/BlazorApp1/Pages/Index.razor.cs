using BlazorApp1.Store;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace BlazorApp1.Pages;

public partial class Index
{
    //private async ValueTask<ItemsProviderResult<int>> LoadEmployees(
    //    ItemsProviderRequest request)
    //{
    //    var test = Enumerable.Range(0, 1000);
    //    if(1000 - request.StartIndex > 0)
    //    {
    //        var numEmployees = Math.Min(request.Count, 1000 - request.StartIndex);
    //        var employees = test.Skip(request.StartIndex).Take(numEmployees);

    //        return new ItemsProviderResult<int>(employees, test.Count());
    //    }
    //    return new ItemsProviderResult<int>(Enumerable.Empty<int>(), 0);
    //}
    [Inject] public required IState<CountState> State { get; init; }
    [Inject] public required IState<CountState2> State2 { get; init; }
    [Inject] public required IDispatcher Dispatcher { get; init; }
    [Inject] public required IActionSubscriber ActionSubscriber { get; init; }
    public List<Token> SQLTokens { get; private set; } = new();
    protected override void OnInitialized()
    {
        //State.StateChanged += (_, __) =>
        //{
        //    Dispatcher.Dispatch(new PostIncrementCountAction());
        //    StateHasChanged();
        //};
        //State2.StateChanged += (_, __) =>
        //{
        //    StateHasChanged();
        //};
        //ActionSubscriber.SubscribeToAction<IncrementCountAction>(this, (action) =>
        //{
        //    Console.WriteLine("123");
        //    //Dispatcher.Dispatch(new PostIncrementCountAction());
        //});
        string sql = "SELECT p.FirstName, p.LastName, p.Total\r\nFROM dbo.Orders as p\r\nWHERE Total = (SELECT MAX(Total) FROM Orders) Order by p.Total desc;";
        ParseResult result = Parser.Parse(sql);

        //ParseResult result2 = Parser.IncrementalParse(sql, null);
        //ParseResult result3 = Parser.IncrementalParse(sql, result2);
        //if (result.Errors.Any())
        //{
        //    foreach (Error error in result.Errors)
        //    {
        //        Console.WriteLine(error.Message);
        //    }
        //}
        SQLTokens = result.Script.Tokens.ToList();
        
        base.OnInitialized();
    }



    public void Add()
    {
        Dispatcher.Dispatch(new IncrementCountAction());
    }
}