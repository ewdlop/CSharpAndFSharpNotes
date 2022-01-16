using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;

using Microsoft.Extensions.Logging;

namespace RealTimeFunctionApp
{
    public static class Function1
    {

        //private readonly TelemetryClient _telemetryClient;

        //public Function1(TelemetryConfiguration configuration)
        //{
        //    _telemetryClient = new TelemetryClient(configuration);
        //}
        [FunctionName("Function1")]
        [SignalROutput(HubName = "realtimehub", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public static async Task Run([BlobTrigger("realtime-container/{name}", Connection = "AzureStorageConnectionString")] Stream myBlob, string name, ILogger log)
        {
            string message = $"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes";
            log.LogInformation(message);
            await Task.FromResult(new MyMessage()
            {
                Target = "ReceiveMessage",
                Arguments = new[] { message }
            });
        }
    }

    public class MyMessage
    {
        public string Target { get; set; }

        public object[] Arguments { get; set; }
    }
}
