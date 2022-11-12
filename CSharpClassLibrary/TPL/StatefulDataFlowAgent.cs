using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CSharpClassLibrary.DataFlow;

public interface IAgent<TMessage>
{
    Task SendAsync(TMessage message);
    void Post(TMessage message);
}
public interface IReplyAgent<TMessage,TReply>
{
    Task<TReply> Ask(TMessage message);
    void Post(TMessage message);
}

public static partial class DataFlowAgent
{
    public static void Run()
    {
        List<string> urls = new List<string> 
        {
            @"http://www.google.com",
            @"http://www.microsoft.com",
            @"http://www.bing.com",
            @"http://www.google.com"
        };
        IAgent<string> agentStateful = StatefulDataFlowAgent<ImmutableDictionary<string, string>,string>
            .Start(ImmutableDictionary<string, string>.Empty,
                    async (state,url) => 
                    {
                        if (!state.TryGetValue(url, out string? content))
                            using (HttpClient httpClient = new HttpClient())
                            {
                                content = await httpClient.GetStringAsync(url);
                                await File.WriteAllTextAsync(CreateFileNameFromUrl(url), content);
                                return state.Add(url, content);
                            }
                        return state;
                    });
        urls.ForEach(url => agentStateful.Post(url));

        //IAgent<string> printer = StatefulDataFlowAgent<string,string>.Start<string,string>((string msg) =>
        //  WriteLine($"{msg} on thread {Environment.CurrentManagedThreadId}"));
    }

    private static string CreateFileNameFromUrl(this string url)
    {
        return url.Replace("http://", string.Empty).Replace(".", "_") + ".html";
    }

}
public class StatefulDataFlowAgent<TState, TMessage> : IAgent<TMessage>
{
    public TState State { get; private set; }
    private readonly ActionBlock<TMessage> actionBlock;
    public StatefulDataFlowAgent(
        TState initialState,
        Func<TState, TMessage, Task<TState>> action,
        CancellationTokenSource? cts = default)
    {
        State = initialState;
        ExecutionDataflowBlockOptions options = new ExecutionDataflowBlockOptions
        {
            CancellationToken = cts != null ? cts.Token : CancellationToken.None
        };
        actionBlock = new ActionBlock<TMessage>(
            async msg => State = await action(State, msg), 
            options);
    }

    public static StatefulDataFlowAgent<TState, TMessage> Start(TState initialState,
         Func<TState, TMessage, Task<TState>> action,
         CancellationTokenSource? cts = default) =>
            new StatefulDataFlowAgent<TState, TMessage>(initialState, action, cts);
    
    public Task SendAsync(TMessage message) => actionBlock.SendAsync(message);
    public void Post(TMessage message) => actionBlock.Post(message);
}