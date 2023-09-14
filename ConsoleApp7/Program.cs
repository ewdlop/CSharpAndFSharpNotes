// Create a ServiceBusClient that will authenticate through Active Directory
using Azure;
using Azure.Core.Amqp;
using Azure.Identity;
using Azure.Messaging;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Transactions;
using System.Xml;
using static System.Formats.Asn1.AsnWriter;
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

//Claim check pattern
{
    string queueName = "<queue_name>";
    var containerClient = new BlobContainerClient("<storage connection string>", "claim-checks");
    await containerClient.CreateIfNotExistsAsync();
    //byte[] body = ServiceBusTestUtilities.GetRandomBuffer(1000000);
    byte[] body = Encoding.UTF8.GetBytes("Hello world");
    string blobName = Guid.NewGuid().ToString();
    await containerClient.UploadBlobAsync(blobName, new BinaryData(body));
    var message = new ServiceBusMessage
    {
        ApplicationProperties =
        {
            ["blob-name"] = blobName
    }
    };

    var client = new ServiceBusClient("<service bus connection string>");
    ServiceBusSender sender = client.CreateSender(queueName);
    await sender.SendMessageAsync(message);

    ServiceBusReceiver receiver = client.CreateReceiver(queueName);
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
    if (receivedMessage.ApplicationProperties.TryGetValue("blob-name", out object blobNameReceived))
    {
        var blobClient = new BlobClient("<storage connection string>", "claim-checks", (string)blobNameReceived);
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        BinaryData messageBody = downloadResult.Content;

        // Once we determine that we are done with the message, we complete it and delete the corresponding blob.
        await receiver.CompleteMessageAsync(receivedMessage);
        await blobClient.DeleteAsync();
    }
}
//Integrating with the CloudEvent type
{
    string connectionString = "<connection_string>";
    string queueName = "<queue_name>";

    // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
    await using var client = new ServiceBusClient(connectionString);

    // create the sender
    ServiceBusSender sender = client.CreateSender(queueName);

    // create a payload using the CloudEvent type
    var cloudEvent = new CloudEvent(
        "/cloudevents/example/source",
        "Example.Employee",
        new Employee { Name = "Homer", Age = 39 });
    ServiceBusMessage message = new ServiceBusMessage(new BinaryData(cloudEvent))
    {
        ContentType = "application/cloudevents+json"
    };

    // send the message
    await sender.SendMessageAsync(message);

    // create a receiver that we can use to receive and settle the message
    ServiceBusReceiver receiver = client.CreateReceiver(queueName);

    // receive the message
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    // deserialize the message body into a CloudEvent
    CloudEvent? receivedCloudEvent = CloudEvent.Parse(receivedMessage.Body);

    // deserialize to our Employee model
    Employee? receivedEmployee = receivedCloudEvent?.Data?.ToObjectFromJson<Employee>();

    // prints 'Homer'
    Console.WriteLine(receivedEmployee?.Name);

    // prints '39'
    Console.WriteLine(receivedEmployee?.Age);

    // complete the message, thereby deleting it from the service
    await receiver.CompleteMessageAsync(receivedMessage);
}
//Managing rules
{
    string connectionString = "<connection_string>";
    string topicName = "<topic_name>";
    string subscriptionName = "<subscription_name>";

    await using var client = new ServiceBusClient(connectionString);

    await using ServiceBusRuleManager ruleManager = client.CreateRuleManager(topicName, subscriptionName);

    // By default, subscriptions are created with a default rule that always evaluates to True. In order to filter, we need
    // to delete the default rule. You can skip this step if you create the subscription with the ServiceBusAdministrationClient,
    // and specify a the FalseRuleFilter in the create rule options.
    await ruleManager.DeleteRuleAsync(RuleProperties.DefaultRuleName);
    await ruleManager.CreateRuleAsync("brand-filter", new CorrelationRuleFilter { Subject = "Toyota" });

    // create the sender
    ServiceBusSender sender = client.CreateSender(topicName);

    ServiceBusMessage[] messages =
    {
        new ServiceBusMessage { Subject = "Ford", ApplicationProperties = { { "Price", 25000 } } },
        new ServiceBusMessage { Subject = "Toyota", ApplicationProperties = { { "Price", 28000 } } },
        new ServiceBusMessage { Subject = "Honda", ApplicationProperties = { { "Price", 35000 } } }
    };

    // send the messages
    await sender.SendMessagesAsync(messages);

    // create a receiver for our subscription that we can use to receive and settle the message
    ServiceBusReceiver receiver = client.CreateReceiver(topicName, subscriptionName);

    // receive the message - we only get back the Toyota message
    while (true)
    {
        ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
        if (receivedMessage == null)
        {
            break;
        }
        Console.WriteLine($"Brand: {receivedMessage.Subject}, Price: {receivedMessage.ApplicationProperties["Price"]}");
        await receiver.CompleteMessageAsync(receivedMessage);
    }

    await ruleManager.CreateRuleAsync("price-filter", new SqlRuleFilter("Price < 30000"));
    await ruleManager.DeleteRuleAsync("brand-filter");

    // we can also use the rule manager to iterate over the rules on the subscription.
    await foreach (RuleProperties rule in ruleManager.GetRulesAsync())
    {
        // we should only have 1 rule at this point - "price-filter"
        Console.WriteLine(rule.Name);
    }

    // send the messages again - because the subscription rules are evaluated when the messages are first enqueued, adding rules
    // for messages that are already in a subscription would have no effect.
    await sender.SendMessagesAsync(messages);

    // receive the messages - we get back both the Ford and the Toyota
    while (true)
    {
        ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
        if (receivedMessage is null)
        {
            break;
        }
        Console.WriteLine($"Brand: {receivedMessage.Subject}, Price: {receivedMessage.ApplicationProperties["Price"]}");
    }
}

//Advanced configuration
{
    string connectionString = "<connection_string>";
    ServiceBusClient client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets,
        WebProxy = new WebProxy("https://myproxyserver:80")
    });
}

//Initiating the connection with a custom endpoint
{
    // Connect to the service using a custom endpoint
    string connectionString = "<connection_string>";
    string customEndpoint = "<custom_endpoint>";

    var options = new ServiceBusClientOptions
    {
        CustomEndpointAddress = new Uri(customEndpoint)
    };

    ServiceBusClient client = new ServiceBusClient(connectionString, options);
}
//Customizing the retry options
{
    string connectionString = "<connection_string>";
    ServiceBusClient client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
    {
        RetryOptions = new ServiceBusRetryOptions
        {
            TryTimeout = TimeSpan.FromSeconds(60),
            MaxRetries = 3,
            Delay = TimeSpan.FromSeconds(.8)
        }
    });
}
//Using prefetch
{
    string connectionString = "<connection_string>";
    ServiceBusClient client = new ServiceBusClient(connectionString);
    ServiceBusReceiver receiver = client.CreateReceiver("<queue-name>", new ServiceBusReceiverOptions
    {
        PrefetchCount = 10
    });
}
{
    string connectionString = "<connection_string>";
    ServiceBusClient client = new ServiceBusClient(connectionString);
    ServiceBusProcessor processor = client.CreateProcessor("<queue-name>", new ServiceBusProcessorOptions
    {
        PrefetchCount = 10
    });
}
//Configuring a lock lost handler when using the processor
{
    string connectionString = "<connection_string>";
    var client = new ServiceBusClient(connectionString);

    // create a processor that we can use to process the messages
    await using ServiceBusProcessor processor = client.CreateProcessor("<queue-name>");

    // configure the message and error handler to use
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    Task SomeExpensiveProcessingAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        // simulate some expensive processing
        return Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(args.CancellationToken);

        try
        {
            args.MessageLockLostAsync += MessageLockLostHandler;

            // We thread our linked token through to our expensive processing so that we can cancel it in the event of a lock lost exception,
            // or when the processor is being stopped.
            await SomeExpensiveProcessingAsync(args.Message, cts.Token);
        }
        finally
        {
            // Finally, we remove the handler to avoid a memory leak.
            args.MessageLockLostAsync -= MessageLockLostHandler;
        }

        Task MessageLockLostHandler(MessageLockLostEventArgs lockLostArgs)
        {
            // We have access to the exception, if any, that triggered the lock lost event.
            // If no exception was provided, the lock was considered lost by the client based on the lock expiry time.
            Console.WriteLine(lockLostArgs.Exception);
            cts.Cancel();
            return Task.CompletedTask;
        }
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

    // since the processing happens in the background, we add a Console.ReadKey to allow the processing to continue until a key is pressed.
    Console.ReadKey();
}
//using ServiceBusSessionProcessor
{
    string connectionString = "<connection_string>";
    var client = new ServiceBusClient(connectionString);

    // create a processor that we can use to process the messages
    await using ServiceBusSessionProcessor processor = client.CreateSessionProcessor("<queue-name>");

    // configure the message and error handler to use
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    Task SomeExpensiveProcessingAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        // simulate some expensive processing
        return Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    async Task MessageHandler(ProcessSessionMessageEventArgs args)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(args.CancellationToken);

        try
        {
            args.SessionLockLostAsync += SessionLockLostHandler;

            // We thread our linked token through to our expensive processing so that we can cancel it in the event of a lock lost exception,
            // or when the processor is being stopped.
            await SomeExpensiveProcessingAsync(args.Message, cts.Token);
        }
        finally
        {
            // Finally, we remove the handler to avoid a memory leak.
            args.SessionLockLostAsync -= SessionLockLostHandler;
        }

        Task SessionLockLostHandler(SessionLockLostEventArgs lockLostArgs)
        {
            // We have access to the exception, if any, that triggered the lock lost event.
            // If no exception was provided, the lock was considered lost by the client based on the lock expiry time.
            Console.WriteLine(lockLostArgs.Exception);
            cts.Cancel();
            return Task.CompletedTask;
        }
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

    // since the processing happens in the background, we add a Console.ReadKey to allow the processing to continue until a key is pressed.
    Console.ReadKey();
}

//Message body
{
    string queueName = "<queue_name>";
    string connectionString = "<connection_string>";
    ServiceBusClient client = new ServiceBusClient(connectionString);

    ServiceBusReceiver receiver = client.CreateReceiver(queueName);
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    AmqpAnnotatedMessage amqpMessage = receivedMessage.GetRawAmqpMessage();
    if (amqpMessage.Body.TryGetValue(out object? value))
    {
        // handle the value body
    }
    else if (amqpMessage.Body.TryGetSequence(out IEnumerable<IList<object>>? sequence))
    {
        // handle the sequence body
    }
    else if (amqpMessage.Body.TryGetData(out IEnumerable<ReadOnlyMemory<byte>>? data))
    {
        // handle the data body - note that unlike when accessing the Body property of the received message,
        // we actually get back a list of byte arrays, not a single byte array. If you were to access the Body property,
        // the data would be flattened into a single byte array.
    }

    ServiceBusSender sender = client.CreateSender(queueName);

    var message = new ServiceBusMessage();
    message.GetRawAmqpMessage().Body = AmqpMessageBody.FromValue(42);
    await sender.SendMessageAsync(message);
}

//Setting miscellaneous properties
{
    string queueName = "<queue_name>";
    string connectionString = "<connection_string>";
    ServiceBusClient client = new ServiceBusClient(connectionString);
    ServiceBusSender sender = client.CreateSender(queueName);

    var message = new ServiceBusMessage("message with AMQP properties set");
    AmqpAnnotatedMessage amqpMessage = message.GetRawAmqpMessage();

    // set some properties of the AMQP header
    amqpMessage.Header.Durable = true;
    amqpMessage.Header.Priority = 1;

    // set some custom properties in the footer
    amqpMessage.Footer["custom-footer-property"] = "custom-footer-value";

    // set some custom properties in the message annotations
    amqpMessage.MessageAnnotations["custom-message-annotation"] = "custom-message-annotation-value";

    // set some custom properties in the delivery annotations
    amqpMessage.DeliveryAnnotations["custom-delivery-annotation"] = "custom-delivery-annotation-value";
    await sender.SendMessageAsync(message);

    ServiceBusReceiver receiver = client.CreateReceiver(queueName);
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    AmqpAnnotatedMessage receivedAmqpMessage = receivedMessage.GetRawAmqpMessage();
    AmqpMessageHeader header = receivedAmqpMessage.Header;

    bool? durable = header.Durable;
    byte? priority = header.Priority;
    string customFooterValue = (string)receivedAmqpMessage.Footer["custom-footer-property"];
    string customMessageAnnotation = (string)receivedAmqpMessage.MessageAnnotations["custom-message-annotation"];
    string customDeliveryAnnotation = (string)receivedAmqpMessage.DeliveryAnnotations["custom-delivery-annotation"];
}

class Employee
{
    public string Name { get; set; }
    public int Age { get; set; }
}
//Extensibility
public class PluginSender : ServiceBusSender
{
    private IEnumerable<Func<ServiceBusMessage, Task>> _plugins;

    internal PluginSender(string queueOrTopicName, ServiceBusClient client, IEnumerable<Func<ServiceBusMessage, Task>> plugins) : base(client, queueOrTopicName)
    {
        _plugins = plugins;
    }

    public override async Task SendMessageAsync(ServiceBusMessage message, CancellationToken cancellationToken = default)
    {
        foreach (var plugin in _plugins)
        {
            await plugin.Invoke(message);
        }
        await base.SendMessageAsync(message, cancellationToken).ConfigureAwait(false);
    }
}

public class PluginReceiver : ServiceBusReceiver
{
    private IEnumerable<Func<ServiceBusReceivedMessage, Task>> _plugins;

    internal PluginReceiver(string queueName, ServiceBusClient client, IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins, ServiceBusReceiverOptions options) :
        base(client, queueName, options)
    {
        _plugins = plugins;
    }

    internal PluginReceiver(string topicName, string subscriptionName, ServiceBusClient client, IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins, ServiceBusReceiverOptions options) :
        base(client, topicName, subscriptionName, options)
    {
        _plugins = plugins;
    }

    public override async Task<ServiceBusReceivedMessage> ReceiveMessageAsync(TimeSpan? maxWaitTime = null, CancellationToken cancellationToken = default)
    {
        ServiceBusReceivedMessage message = await base.ReceiveMessageAsync(maxWaitTime, cancellationToken).ConfigureAwait(false);

        foreach (var plugin in _plugins)
        {
            await plugin.Invoke(message);
        }
        return message;
    }
}

public class PluginProcessor : ServiceBusProcessor
{
    private IEnumerable<Func<ServiceBusReceivedMessage, Task>> _plugins;

    internal PluginProcessor(string queueName, ServiceBusClient client, IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins, ServiceBusProcessorOptions options) :
        base(client, queueName, options)
    {
        _plugins = plugins;
    }

    internal PluginProcessor(string topicName, string subscriptionName, ServiceBusClient client, IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins, ServiceBusProcessorOptions options) :
        base(client, topicName, subscriptionName, options)
    {
        _plugins = plugins;
    }

    protected override async Task OnProcessMessageAsync(ProcessMessageEventArgs args)
    {
        foreach (var plugin in _plugins)
        {
            await plugin.Invoke(args.Message);
        }

        await base.OnProcessMessageAsync(args);
    }
}

public class PluginSessionProcessor : ServiceBusSessionProcessor
{
    private IEnumerable<Func<ServiceBusReceivedMessage, Task>> _plugins;
    private IEnumerable<Func<Exception, Task>> _errorPlugins;

    internal PluginSessionProcessor(string queueName, ServiceBusClient client, IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins, ServiceBusSessionProcessorOptions options) :
        base(client, queueName, options)
    {
        _plugins = plugins;
    }

    internal PluginSessionProcessor(string topicName, string subscriptionName, ServiceBusClient client, IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins, ServiceBusSessionProcessorOptions options) :
        base(client, topicName, subscriptionName, options)
    {
        _plugins = plugins;
    }

    protected override async Task OnProcessSessionMessageAsync(ProcessSessionMessageEventArgs args)
    {
        foreach (var plugin in _plugins)
        {
            await plugin.Invoke(args.Message);
        }

        await base.OnProcessSessionMessageAsync(args);
    }

    protected override async Task OnProcessErrorAsync(ProcessErrorEventArgs args)
    {
        foreach (var errorPlugin in _errorPlugins)
        {
            await errorPlugin.Invoke(args.Exception);
        }
        await base.OnProcessErrorAsync(args);
    }
}

public static class ServiceBusClientExtension
{
    public static PluginSender CreatePluginSender(
    this ServiceBusClient client,
    string queueOrTopicName,
    IEnumerable<Func<ServiceBusMessage, Task>> plugins)
    {
        return new PluginSender(queueOrTopicName, client, plugins);
    }

    public static PluginReceiver CreatePluginReceiver(
        this ServiceBusClient client,
        string queueName,
        IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins,
        ServiceBusReceiverOptions options = default)
    {
        return new PluginReceiver(queueName, client, plugins, options ?? new ServiceBusReceiverOptions());
    }

    public static PluginReceiver CreatePluginReceiver(
        this ServiceBusClient client,
        string topicName,
        string subscriptionName,
        IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins,
        ServiceBusReceiverOptions options = default)
    {
        return new PluginReceiver(topicName, subscriptionName, client, plugins, options ?? new ServiceBusReceiverOptions());
    }

    public static PluginProcessor CreatePluginProcessor(
        this ServiceBusClient client,
        string queueName,
        IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins,
        ServiceBusProcessorOptions options = default)
    {
        return new PluginProcessor(queueName, client, plugins, options ?? new ServiceBusProcessorOptions());
    }

    public static PluginProcessor CreatePluginProcessor(
        this ServiceBusClient client,
        string topicName,
        string subscriptionName,
        IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins,
        ServiceBusProcessorOptions options = default)
    {
        return new PluginProcessor(topicName, subscriptionName, client, plugins, options ?? new ServiceBusProcessorOptions());
    }

    public static PluginSessionProcessor CreatePluginSessionProcessor(
        this ServiceBusClient client,
        string queueName,
        IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins,
        ServiceBusSessionProcessorOptions options = default)
    {
        return new PluginSessionProcessor(queueName, client, plugins, options ?? new ServiceBusSessionProcessorOptions());
    }

    public static PluginSessionProcessor CreatePluginSessionProcessor(
        this ServiceBusClient client,
        string topicName,
        string subscriptionName,
        IEnumerable<Func<ServiceBusReceivedMessage, Task>> plugins,
        ServiceBusSessionProcessorOptions options = default)
    {
        return new PluginSessionProcessor(topicName, subscriptionName, client, plugins, options ?? new ServiceBusSessionProcessorOptions());
    }
}


public class TestModel
{
    public string A { get; set; }
    public int B { get; set; }
    public bool C { get; set; }
}