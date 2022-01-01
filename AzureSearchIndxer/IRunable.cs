namespace AzureSearchIndxer
{
    internal interface IRunable
    {
        Task Run(CancellationToken cancellationToken);
        Task Stop(CancellationToken cancellationToken);
    }
}