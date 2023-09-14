// Create a ServiceBusClient that will authenticate through Active Directory
using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Transactions;
using System.Xml;
{
    string fullyQualifiedNamespace = "yournamespace.servicebus.windows.net";
    await using ServiceBusClient client = new ServiceBusClient(fullyQualifiedNamespace, new DefaultAzureCredential());

    // Create a ServiceBusClient that will authenticate through a Shared Access Key
    AzureNamedKeyCredential credential = new AzureNamedKeyCredential("<name>", "<key>");
    await using ServiceBusClient client2 = new ServiceBusClient("yournamespace.servicebus.windows.net", credential);

    // Create a ServiceBusClient that will authenticate through a Shared Access Signature

    string keyName = "<key_name>";
    string key = "<key>";
    string queueName = "<queue_name>";
    using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
    UriBuilder builder = new UriBuilder(fullyQualifiedNamespace)
    {
        Scheme = "amqps",
        // scope our SAS token to the queue that is being used to adhere to the principle of least privilege
        Path = queueName
    };

    string url = WebUtility.UrlEncode(builder.Uri.AbsoluteUri);
    long exp = DateTimeOffset.Now.AddHours(1).ToUnixTimeSeconds();
    string sig = WebUtility.UrlEncode(Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(url + "\n" + exp))));

    string sasToken = $"SharedAccessSignature sr={url}&sig={sig}&se={exp}&skn={keyName}";

    AzureSasCredential sasCredential = new AzureSasCredential(sasToken);
    await using ServiceBusClient client3 = new ServiceBusClient(fullyQualifiedNamespace, sasCredential);

    //Send and receive a message using queues

    string connectionString = "<connection_string>";
    queueName = "<queue_name>";
    // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
    await using var client4 = new ServiceBusClient(connectionString);

    ServiceBusSender sender = client4.CreateSender(queueName);
    ServiceBusMessage message = new ServiceBusMessage("Hello World");

    await sender.SendMessageAsync(message);

    ServiceBusReceiver receiver = client4.CreateReceiver(queueName);
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    // get the message body as a string
    string body = receivedMessage.Body.ToString();
    Console.WriteLine(body);

    //Send and receive a message using topics and subscriptions

    string topicName = "<topic_name>";
    string subscriptionName = "<subscription_name>";
    // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
    await using ServiceBusClient client5 = new ServiceBusClient(connectionString);

    // create the sender that we will use to send to our topic
    ServiceBusSender sender2 = client.CreateSender(topicName);

    // create a message that we can send. UTF-8 encoding is used when providing a string.
    ServiceBusMessage message2 = new ServiceBusMessage("Hello world!");

    // send the message
    await sender2.SendMessageAsync(message2);

    ServiceBusReceiver receiver2 = client5.CreateReceiver(topicName, subscriptionName);
    // the received message is a different type as it contains some service set properties
    ServiceBusReceivedMessage receivedMessage2 = await receiver2.ReceiveMessageAsync();

    // get the message body as a string
    string body2 = receivedMessage2.Body.ToString();
    Console.WriteLine(body2);

    //Send and receive a batch of messages
    IList<ServiceBusMessage> messages = new List<ServiceBusMessage>
{
    new ServiceBusMessage("First"),
    new ServiceBusMessage("Second")
};
    // send the messages
    await sender.SendMessagesAsync(messages);

    // add the messages that we plan to send to a local queue
    Queue<ServiceBusMessage> messages2 = new Queue<ServiceBusMessage>();
    messages2.Enqueue(new ServiceBusMessage("First message"));
    messages2.Enqueue(new ServiceBusMessage("Second message"));
    messages2.Enqueue(new ServiceBusMessage("Third message"));

    // create a message batch that we can send
    // total number of messages to be sent to the Service Bus queue
    int messageCount = messages2.Count;

    // while all messages are not sent to the Service Bus queue
    while (messages2.Count > 0)
    {
        // start a new batch
        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

        // add the first message to the batch
        if (messageBatch.TryAddMessage(messages2.Peek()))
        {
            // dequeue the message from the .NET queue once the message is added to the batch
            messages2.Dequeue();
        }
        else
        {
            // if the first message can't fit, then it is too large for the batch
            throw new Exception($"Message {messageCount - messages2.Count} is too large and cannot be sent.");
        }

        // add as many messages as possible to the current batch
        while (messages2.Count > 0 && messageBatch.TryAddMessage(messages2.Peek()))
        {
            // dequeue the message from the .NET queue as it has been added to the batch
            messages2.Dequeue();
        }

        // now, send the batch
        await sender.SendMessagesAsync(messageBatch);

        // if there are any remaining messages in the .NET queue, the while loop repeats
    }

    //Receiving a batch of messages
    // create a receiver that we can use to receive the messages
    ServiceBusReceiver receiver3 = client.CreateReceiver(queueName);

    // the received message is a different type as it contains some service set properties
    // a batch of messages (maximum of 2 in this case) are received
    IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver3.ReceiveMessagesAsync(maxMessages: 2);

    // go through each of the messages received
    foreach (ServiceBusReceivedMessage receivedMessage1 in receivedMessages)
    {
        // get the message body as a string
        string body3 = receivedMessage1.Body.ToString();
    }

    //Peeking a message
    ServiceBusReceivedMessage peekedMessage = await receiver.PeekMessageAsync();

    //Schedule a message
    long seq = await sender.ScheduleMessageAsync(
        message,
        DateTimeOffset.Now.AddDays(1));

    //Cancel a scheduled message
    await sender.CancelScheduledMessageAsync(seq);

    //Setting time to live
    ServiceBusMessage? message3 = new ServiceBusMessage("Hello world!") { TimeToLive = TimeSpan.FromMinutes(5) };

    //Completing a message
    // create a receiver that we can use to receive and settle the message
    ServiceBusReceiver receiver4 = client.CreateReceiver(queueName);

    // the received message is a different type as it contains some service set properties
    ServiceBusReceivedMessage receivedMessage3 = await receiver4.ReceiveMessageAsync();

    // complete the message, thereby deleting it from the service
    await receiver4.CompleteMessageAsync(receivedMessage3);

    //Abandoning a message
    ServiceBusReceivedMessage receivedMessage4 = await receiver4.ReceiveMessageAsync();

    // abandon the message, thereby releasing the lock and allowing it to be received again by this or other receivers
    await receiver4.AbandonMessageAsync(receivedMessage4);

    //Deferring a message
    ServiceBusReceivedMessage receivedMessage5 = await receiver4.ReceiveMessageAsync();

    // defer the message, thereby preventing the message from being received again by this or other receivers until the deferred message is removed
    await receiver4.DeferMessageAsync(receivedMessage5);

    //Dead-lettering a message
    ServiceBusReceivedMessage receivedMessage6 = await receiver4.ReceiveMessageAsync();

    // dead-letter the message, thereby sending it to the dead-letter queue for this receiver's entity
    await receiver4.DeadLetterMessageAsync(receivedMessage6);

    //Renewing a message lock
    ServiceBusReceivedMessage receivedMessage7 = await receiver4.ReceiveMessageAsync();

    // the received message is a different type as it contains some service set properties
    // renew the message lock
    await receiver4.RenewMessageLockAsync(receivedMessage7);

    //Receiving from next available session
    ServiceBusSessionReceiver receiver5 = await client.AcceptNextSessionAsync(queueName);
    await receiver5.CompleteMessageAsync(receivedMessage5);

    //Receive from a specific session
    // create a receiver specifying a particular session
    ServiceBusSessionReceiver receiver6 = await client.AcceptSessionAsync(queueName, "Session2");

    // the received message is a different type as it contains some service set properties
    ServiceBusReceivedMessage receivedMessag7 = await receiver6.ReceiveMessageAsync();
    Console.WriteLine(receivedMessag7.SessionId);

    // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
    await using var client6 = new ServiceBusClient(connectionString);

    // create the sender
    ServiceBusSender sender3 = client6.CreateSender(queueName);

    // create a session message that we can send
    ServiceBusMessage message4 = new ServiceBusMessage(Encoding.UTF8.GetBytes("Hello world!"))
    {
        SessionId = "mySessionId"
    };

    // send the message
    await sender.SendMessageAsync(message4);

    // create a session receiver that we can use to receive the message. Since we don't specify a
    // particular session, we will get the next available session from the service.
    ServiceBusSessionReceiver receiver7 = await client6.AcceptNextSessionAsync(queueName);

    // the received message is a different type as it contains some service set properties
    ServiceBusReceivedMessage receivedMessage8 = await receiver7.ReceiveMessageAsync();
    Console.WriteLine(receivedMessage8.SessionId);

    // we can also set arbitrary session state using this receiver
    // the state is specific to the session, and not any particular message
    await receiver7.SetSessionStateAsync(new BinaryData("some state"));

    // the state can be retrieved for the session as well
    BinaryData state = await receiver7.GetSessionStateAsync();

    //Settling session messages
    ServiceBusReceivedMessage receivedMessage9 = await receiver7.ReceiveMessageAsync();

    // If we know that we are going to be processing the session for a long time, we can extend the lock for the session
    // by the configured LockDuration (by default, 30 seconds).
    await receiver7.RenewSessionLockAsync();

    // simulate some processing of the message
    await Task.Delay(TimeSpan.FromSeconds(10));

    // complete the message, thereby deleting it from the service
    await receiver.CompleteMessageAsync(receivedMessage);

    //Processing messages
    ServiceBusProcessorOptions options = new ServiceBusProcessorOptions
    {
        // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
        // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
        // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
        AutoCompleteMessages = false,

        // I can also allow for multi-threading
        MaxConcurrentCalls = 2
    };
    // create a processor that we can use to process the messages
    await using ServiceBusProcessor processor = client.CreateProcessor(queueName, options);

    // configure the message and error handler to use
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine(body);

        // we can evaluate application logic and use that to determine how to settle the message.
        await args.CompleteMessageAsync(args.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        // the error source tells me at what point in the processing an error occurred
        Console.WriteLine(args.ErrorSource);
        // the fully qualified namespace is available
        Console.WriteLine(args.FullyQualifiedNamespace);
        // as well as the entity path
        Console.WriteLine(args.EntityPath);
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    // start processing
    await processor.StartProcessingAsync();

    //Processing messages from a session-enabled queue
    // create the sender
    ServiceBusSender sender4 = client.CreateSender(queueName);

    // create a message batch that we can send
    ServiceBusMessageBatch messageBatch2 = await sender4.CreateMessageBatchAsync();
    messageBatch2.TryAddMessage(
        new ServiceBusMessage("First")
        {
            SessionId = "Session1"
        });
    messageBatch2.TryAddMessage(
        new ServiceBusMessage("Second")
        {
            SessionId = "Session2"
        });

    // send the message batch
    await sender.SendMessagesAsync(messageBatch2);

    // create the options to use for configuring the processor
    ServiceBusSessionProcessorOptions options2 = new ServiceBusSessionProcessorOptions
    {
        // By default after the message handler returns, the processor will complete the message
        // If I want more fine-grained control over settlement, I can set this to false.
        AutoCompleteMessages = false,

        // I can also allow for processing multiple sessions
        MaxConcurrentSessions = 5,

        // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
        // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
        // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
        MaxConcurrentCallsPerSession = 2,

        // Processing can be optionally limited to a subset of session Ids.
        SessionIds = { "my-session", "your-session" },
    };

    // create a session processor that we can use to process the messages
    await using ServiceBusSessionProcessor processor2 = client.CreateSessionProcessor(queueName, options2);

    // configure the message and error handler to use
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;
    await processor2.StartProcessingAsync();

    //Sending and completing a message in a transaction on the same entity
    await using ServiceBusClient client8 = new ServiceBusClient(connectionString);
    ServiceBusSender sender8 = client8.CreateSender(queueName);
    await sender8.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes("First")));
    ServiceBusReceiver receiver8 = client8.CreateReceiver(queueName);
    ServiceBusReceivedMessage firstMessage = await receiver8.ReceiveMessageAsync();
    using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        await sender8.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes("Second")));
        await receiver8.CompleteMessageAsync(firstMessage);
        ts.Complete();
    }

    //Setting session state within a transaction
    await using var client9 = new ServiceBusClient(connectionString);
    ServiceBusSender sender9 = client.CreateSender(queueName);

    await sender9.SendMessageAsync(new ServiceBusMessage("my message") { SessionId = "sessionId" });
    ServiceBusSessionReceiver receiver9 = await client.AcceptNextSessionAsync(queueName);
    ServiceBusReceivedMessage receivedMessage10 = await receiver.ReceiveMessageAsync();

    byte[]? state2 = Encoding.UTF8.GetBytes("some state");
    using (TransactionScope? ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        await receiver9.CompleteMessageAsync(receivedMessage10);
        await receiver9.SetSessionStateAsync(new BinaryData(state2));
        ts.Complete();
    }
}

//Transactions across entities
{
    string connectionString = "<connection_string>";
    var options = new ServiceBusClientOptions { EnableCrossEntityTransactions = true };
    await using var client = new ServiceBusClient(connectionString, options);

    ServiceBusReceiver receiverA = client.CreateReceiver("queueA");
    ServiceBusSender senderB = client.CreateSender("queueB");
    ServiceBusSender senderC = client.CreateSender("topicC");

    ServiceBusReceivedMessage receivedMessage = await receiverA.ReceiveMessageAsync();

    using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        await receiverA.CompleteMessageAsync(receivedMessage);
        await senderB.SendMessageAsync(new ServiceBusMessage());
        await senderC.SendMessageAsync(new ServiceBusMessage());
        ts.Complete();
    }
}

//Create a queue
{
    string connectionString = "<connection_string>";
    string queueName = "<queue_name>";
    ServiceBusAdministrationClient client = new ServiceBusAdministrationClient(connectionString);
    CreateQueueOptions options = new CreateQueueOptions(queueName)
    {
        AutoDeleteOnIdle = TimeSpan.FromDays(7),
        DefaultMessageTimeToLive = TimeSpan.FromDays(2),
        DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(1),
        EnableBatchedOperations = true,
        DeadLetteringOnMessageExpiration = true,
        EnablePartitioning = false,
        ForwardDeadLetteredMessagesTo = null,
        ForwardTo = null,
        LockDuration = TimeSpan.FromSeconds(45),
        MaxDeliveryCount = 8,
        MaxSizeInMegabytes = 2048,
        RequiresDuplicateDetection = true,
        RequiresSession = true,
        UserMetadata = "some metadata"
    };

    options.AuthorizationRules.Add(new SharedAccessAuthorizationRule(
        "allClaims",
        new[] { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

    QueueProperties createdQueue = await client.CreateQueueAsync(options);

    //Get a queue
    QueueProperties queue = await client.GetQueueAsync(queueName);

    //Update a queue
    queue.LockDuration = TimeSpan.FromSeconds(60);
    QueueProperties updatedQueue = await client.UpdateQueueAsync(queue);

    //Delete a queue
    await client.DeleteQueueAsync(queueName);
}
//Create a topic and subscription
{
    string connectionString = "<connection_string>";
    string topicName = "<topic_name>";
    ServiceBusAdministrationClient client = new ServiceBusAdministrationClient(connectionString);
    CreateTopicOptions topicOptions = new CreateTopicOptions(topicName)
    {
        AutoDeleteOnIdle = TimeSpan.FromDays(7),
        DefaultMessageTimeToLive = TimeSpan.FromDays(2),
        DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(1),
        EnableBatchedOperations = true,
        EnablePartitioning = false,
        MaxSizeInMegabytes = 2048,
        RequiresDuplicateDetection = true,
        UserMetadata = "some metadata"
    };
    topicOptions.AuthorizationRules.Add(new SharedAccessAuthorizationRule(
        "allClaims",
        new[] { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

    TopicProperties createdTopic = await client.CreateTopicAsync(topicOptions);

    string subscriptionName = "<subscription_name>";
    CreateSubscriptionOptions subscriptionOptions = new CreateSubscriptionOptions(topicName, subscriptionName)
    {
        AutoDeleteOnIdle = TimeSpan.FromDays(7),
        DefaultMessageTimeToLive = TimeSpan.FromDays(2),
        EnableBatchedOperations = true,
        UserMetadata = "some metadata"
    };
    SubscriptionProperties createdSubscription = await client.CreateSubscriptionAsync(subscriptionOptions);

    //Get a topic
    TopicProperties topic = await client.GetTopicAsync(topicName);

    //Get a subscription
    SubscriptionProperties subscription = await client.GetSubscriptionAsync(topicName, subscriptionName);

    //Update a topic
    topic.UserMetadata = "some metadata";
    TopicProperties updatedTopic = await client.UpdateTopicAsync(topic);

    //Update a subscription
    subscription.UserMetadata = "some metadata";
    SubscriptionProperties updatedSubscription = await client.UpdateSubscriptionAsync(subscription);

    //Delete a subscription
    await client.DeleteSubscriptionAsync(topicName, subscriptionName);

    //Delete a topic
    await client.DeleteTopicAsync(topicName);

    string queueName = string.Empty;
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    //Sending a message using Azure.Messaging.ServiceBus that will be received with WindowsAzure.ServiceBus
    {
        ServiceBusSender sender = serviceBusClient.CreateSender(queueName);
        // When constructing the `DataContractSerializer`, We pass in the type for the model, which can be a strongly typed model or some pre-serialized data.
        // If you use a strongly typed model here, the model properties will be serialized into XML. Since JSON is more commonly used, we will use it in our example, and
        // and specify the type as string, since we will provide a JSON string.
        var serializer = new DataContractSerializer(typeof(string));
        using var stream = new MemoryStream();
        XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(stream);

        // serialize an instance of our type into a JSON string
        string json = JsonSerializer.Serialize(new TestModel { A = "Hello world", B = 5, C = true });

        // serialize our JSON string into the XML envelope using the DataContractSerializer
        serializer.WriteObject(writer, json);
        writer.Flush();

        // construct the ServiceBusMessage using the DataContract serialized JSON
        var message = new ServiceBusMessage(stream.ToArray());

        await sender.SendMessageAsync(message);
    }

    {
        ServiceBusReceiver receiver = serviceBusClient.CreateReceiver(queueName);
        ServiceBusReceivedMessage received = await receiver.ReceiveMessageAsync();

        // Similar to the send scenario, we still rely on the DataContractSerializer and we use string as our type because we are expecting a JSON
        // message body.
        var deserializer = new DataContractSerializer(typeof(string));
        XmlDictionaryReader reader =
            XmlDictionaryReader.CreateBinaryReader(received.Body.ToStream(), XmlDictionaryReaderQuotas.Max);

        // deserialize the XML envelope into a string
        string receivedJson = (string)deserializer.ReadObject(reader);

        // deserialize the JSON string into TestModel
        TestModel? output = JsonSerializer.Deserialize<TestModel>(receivedJson);
    }
}

public class TestModel
{
    public string A { get; set; }
    public int B { get; set; }
    public bool C { get; set; }
}