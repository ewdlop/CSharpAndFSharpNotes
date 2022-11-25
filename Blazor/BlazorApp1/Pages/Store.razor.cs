using Fluxor;
using Microsoft.AspNetCore.Components;
using Fluxor.Blazor.Web.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace BlazorApp1.Pages;

[FeatureState]
public record UrlStoreState(string[] Items)
{
    public UrlStoreState(): this(Array.Empty<string>()) {}
}
public record UrlStoreReadAction();
public record UrlStoreWriteAction(string[] Items);
public record UrlStoreAddAction(string Item);
public static class Reducers
{
    [ReducerMethod]
    public static UrlStoreState ReduceWriteAction(UrlStoreState state, UrlStoreWriteAction urlStoreWriteAction)
    {
        return state with { Items = urlStoreWriteAction.Items };
    }
    [ReducerMethod]
    public static UrlStoreState ReduceStoreAddAction(UrlStoreState state, UrlStoreAddAction urlStoreAddAction)
    {
        return state with { Items = state.Items.Append(urlStoreAddAction.Item).ToArray() };
    }
}
public partial class Store : FluxorComponent
{
    [Inject] public required IState<UrlStoreState> UrlStoreState { get; init; }
    [Inject] public required IJSRuntime JSRuntime { get; init; }
    [Inject] public required IDispatcher Dispatcher { get; init; }
    [Inject] public required NavigationManager NavigationManager { get; init; }
    [Inject] public required ILogger<Store> Logger { get; init; }
    [Parameter][SupplyParameterFromQuery(Name = "state")] public string? State { get; init; }
    public string NormalText { get; set; } = string.Empty;
    private string PreviousBase64UrlString { get; set; } = string.Empty;

    protected override void OnAfterRender(bool firstRender)
    {
        if(firstRender)
        {
            ReadUrl();
            //need to wait for jsinterop to be ready
            UrlStoreState.StateChanged += UrlStoreStateChanged;
            
        }
        base.OnAfterRender(firstRender);
    }

    private async void UrlStoreStateChanged(object? _, EventArgs __)
    {
        byte[] utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(UrlStoreState.Value.Items);
        if(utf8Bytes is not null && utf8Bytes.Any())
        {
            string currentBase64UrlString = WebEncoders.Base64UrlEncode(utf8Bytes);
            if(!PreviousBase64UrlString.Equals(currentBase64UrlString))
            {
                if (!string.IsNullOrEmpty(currentBase64UrlString))
                {
                    PreviousBase64UrlString = currentBase64UrlString;
                    string uri = NavigationManager.GetUriWithQueryParameter("state", currentBase64UrlString);
                    await JSRuntime.InvokeVoidAsync("ChangeUrl", uri);
                }
            }
            
        }
    }

    private void ReadUrl()
    {
        //from Blazor Component Parameter
        if (!string.IsNullOrWhiteSpace(State)/* && IsBase64String(State, out _)*/)
        {
            try
            {
                byte[] utf8Bytes = WebEncoders.Base64UrlDecode(State);
                string[]? state = JsonSerializer.Deserialize<string[]>(utf8Bytes);
                if (state is not null)
                {
                    Dispatcher.Dispatch(new UrlStoreWriteAction(state));
                }
            }
            catch(FormatException e)
            {
                Logger.LogCritical(e, "{Message}", e.Message);
            }
            catch(JsonException e)
            {
                Logger.LogCritical(e, "{Message}", e.Message);
            }
            catch (NotSupportedException e)
            {
                Logger.LogCritical(e, "{Message}", e.Message);
            }
            catch (Exception e)
            {
                Logger.LogCritical(e, "{Message}", e.Message);
            }
        }
    }

    private void Add()
    {
        Dispatcher.Dispatch(new UrlStoreAddAction(NormalText));
    }

    /// <summary>
    /// maybe needed
    /// </summary>
    /// <param name="base64"></param>
    /// <param name="bytesParsed"></param>
    /// <returns></returns>
    private static bool IsBase64String(string base64, out int bytesParsed )
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out bytesParsed);
    }
}