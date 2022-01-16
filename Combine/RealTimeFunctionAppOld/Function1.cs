using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RealTimeFunctionAppOld
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static Task Run([BlobTrigger("realtime-container/{name}", Connection = "AzureStorageConnectionString")]Stream myBlob, string name, ILogger log,
            [SignalR(HubName = "realtimehub", ConnectionStringSetting = "AzureSignalRConnectionString")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            string message = $"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes";
            log.LogInformation(message);
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "ReceiveMessage",
                    Arguments = new[] { message }
                });
        }
    }
}
