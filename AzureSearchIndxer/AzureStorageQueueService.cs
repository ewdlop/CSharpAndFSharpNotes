using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.WindowsAzure.Storage;
using System.Diagnostics;

namespace AzureSearchIndxer
{
    internal static class AzureStorageQueueService
    {
        public static async Task<string> RetrieveNextMessageAsync(QueueClient theQueue)
        {
            if (await theQueue.ExistsAsync())
            {
                QueueProperties properties = await theQueue.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await theQueue.ReceiveMessagesAsync(1).Result.r;
                    string theMessage = retrievedMessage[0].Body.ToString();
                    await theQueue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    return theMessage;
                }
                else
                {
                    Console.Write("The queue is empty. Attempt to delete it? (Y/N) ");
                    string response = Console.ReadLine();

                    if (response.ToUpper() == "Y")
                    {
                        await theQueue.DeleteIfExistsAsync();
                        return "The queue was deleted.";
                    }
                    else
                    {
                        return "The queue was not deleted.";
                    }
                }
            }
            else
            {
                return "The queue does not exist. Add a message to the command line to create the queue and store the message.";
            }
        }
        public static async Task<bool> SafeCreateIfNotExistsAsync(this QueueClient queueClient, TimeSpan? timeout, CancellationToken cancellationToken = default)
        {
            Stopwatch sw = Stopwatch.StartNew();
            if (timeout == null) timeout = TimeSpan.FromSeconds(41); // Assuming 40 seconds max time, and then some.
            do
            {
                if (sw.Elapsed > timeout.Value) throw new TimeoutException("Table was not deleted within the timeout.");

                try
                {
                    var response = await queueClient.CreateIfNotExistsAsync();
                    if(response.Status == 201)
                    {
                        return true;
                    }
                }
                catch (StorageException e) when (IsAzureQueueBeingDeleted(e))
                {
                    // The table is currently being deleted. Try again until it works.
                    await Task.Delay(1000);
                }
            } while (true);
        }

        private static bool IsAzureQueueBeingDeleted(StorageException e)
        {
            return
                e.RequestInformation.HttpStatusCode == 409
                &&
                e.RequestInformation.ExtendedErrorInformation.ErrorCode.Equals(QueueErrorCode.QueueBeingDeleted);
        }
    }
}
}
