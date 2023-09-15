// <using_directives> 
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Net;
using Container = Microsoft.Azure.Cosmos.Container;

// </using_directives>
{
    // <endpoint_key> 
    // New instance of CosmosClient class using an endpoint and key string
    using CosmosClient client = new(
        accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
        authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
    );
    // </endpoint_key>

    // <create_database>
    // New instance of Database class referencing the server-side database
    Database database = await client.CreateDatabaseIfNotExistsAsync(
        id: "adventureworks"
    );
    // </create_database>

    // <create_container>
    // New instance of Container class referencing the server-side container
    Container container = await database.CreateContainerIfNotExistsAsync(
        id: "products",
        partitionKeyPath: "/category",
        throughput: 400
    );
    // </create_container>

    // <create_object> 
    // Create new item and add to container
    Product item = new(
        Id: "68719518388",
        Category: "gear-surf-surfboards",
        Name: "Sunnox Surfboard",
        Quantity: 8,
        Sale: true
    );
    // </create_object> 

    // <create_item> 
    Product createdItem = await container.CreateItemAsync<Product>(
        item: item,
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );
    // </create_item> 

    // <replace_item>
    Product replacedItem = await container.ReplaceItemAsync<Product>(
        item: item,
        id: "68719518388",
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );
    // </replace_item>

    // <upsert_item>
    Product upsertedItem = await container.UpsertItemAsync<Product>(
        item: item,
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );
    // </upsert_item>
}
{
    // New instance of CosmosClient class using an endpoint and key string
    using CosmosClient client = new(
        accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
        authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
    );
    // </endpoint_key>

    // <create_database>
    // New instance of Database class referencing the server-side database
    Database database = await client.CreateDatabaseIfNotExistsAsync(
        id: "adventureworks"
    );
    // </create_database>

    // <create_container>
    // New instance of Container class referencing the server-side container
    Container container = await database.CreateContainerIfNotExistsAsync(
        id: "products",
        partitionKeyPath: "/category",
        throughput: 400
    );
    // </create_container>

    // <create_objects> 
    // Create new item and add to container
    Product firstItem = new(
        Id: "68719518388",
        Category: "gear-surf-surfboards",
        Name: "Sunnox Surfboard",
        Quantity: 8,
        Sale: true
    );

    Product secondItem = new(
        Id: "68719518381",
        Category: "gear-surf-surfboards",
        Name: "Kalbar Surfboard",
        Quantity: 4,
        Sale: false
    );
    // </create_objects>

    // <create_items>
    await container.CreateItemAsync<Product>(
        item: firstItem,
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );

    await container.CreateItemAsync<Product>(
        item: secondItem,
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );
    // </create_items> 

    // <read_item>
    // Read existing item from container
    Product readItem = await container.ReadItemAsync<Product>(
        id: "68719518388",
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );
    // </read_item> 

    // <read_item_expanded>
    // Read existing item from container
    ItemResponse<Product> readResponse = await container.ReadItemAsync<Product>(
        id: "68719518388",
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );

    // Get response metadata
    double requestUnits = readResponse.RequestCharge;
    HttpStatusCode statusCode = readResponse.StatusCode;

    // Explicitly get item
    Product readItemExplicit = readResponse.Resource;
    // </read_item_expanded>

    // <read_item_stream>
    // Read existing item from container
    using ResponseMessage readItemStreamResponse = await container.ReadItemStreamAsync(
        id: "68719518388",
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );

    // Get stream from response
    using StreamReader readItemStreamReader = new(readItemStreamResponse.Content);

    // (optional) Get stream content
    string content = await readItemStreamReader.ReadToEndAsync();
    // </read_item_stream>

    // <read_multiple_items>
    // Create partition key object
    PartitionKey partitionKey = new("gear-surf-surfboards");

    // Create list of tuples for each item
    List<(string, PartitionKey)> itemsToFind = new()
    {
        ("68719518388", partitionKey),
        ("68719518381", partitionKey)
    };

    // Read multiple items
    FeedResponse<Product> feedResponse = await container.ReadManyItemsAsync<Product>(
        items: itemsToFind
    );

    foreach (Product item in feedResponse)
    {
        Console.WriteLine($"Found item:\t{item.Name}");
    }
    // </read_multiple_items>
}
{
    //300-query-items
    // <endpoint_key> 
    // New instance of CosmosClient class using an endpoint and key string
    using CosmosClient client = new(
        accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
        authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
    );
    // </endpoint_key>

    // <create_database>
    // New instance of Database class referencing the server-side database
    Database database = await client.CreateDatabaseIfNotExistsAsync(
        id: "adventureworks"
    );
    // </create_database>

    // <create_container>
    // New instance of Container class referencing the server-side container
    Container container = await database.CreateContainerIfNotExistsAsync(
        id: "products",
        partitionKeyPath: "/category",
        throughput: 400
    );

    // Create new items and add to container
    Product firstNewItem = new(
        Id: "68719518388",
        Category: "gear-surf-surfboards",
        Name: "Sunnox Surfboard",
        Quantity: 8,
        Sale: true
    );

    Product secondNewitem = new(
        Id: "68719518398",
        Category: "gear-surf-surfboards",
        Name: "Noosa Surfboard",
        Quantity: 15,
        Sale: false
    );

    // Add items to container

    await container.CreateItemAsync<Product>(
        item: firstNewItem,
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );

    await container.CreateItemAsync<Product>(
        item: secondNewitem,
        partitionKey: new PartitionKey("gear-surf-surfboards")
    );

    // Query multiple items from container
    using FeedIterator<Product> feed = container.GetItemQueryIterator<Product>(
        queryText: "SELECT * FROM products"
    );

    while (feed.HasMoreResults)
    {
        FeedResponse<Product> response = await feed.ReadNextAsync();

        // Iterate query results
        foreach (Product item in response)
        {
            Console.WriteLine($"Found item:\t{item.Name}");
        }
    }
    QueryDefinition parameterizedQuery = new QueryDefinition(
        query: "SELECT * FROM products p WHERE p.category = @partitionKey"
    ).WithParameter("@partitionKey", "gear-surf-surfboards");

    // Query multiple items from container
    using FeedIterator<Product> filteredFeed = container.GetItemQueryIterator<Product>(
        queryDefinition: parameterizedQuery
    );

    // Iterate query result pages
    while (filteredFeed.HasMoreResults)
    {
        FeedResponse<Product> response = await filteredFeed.ReadNextAsync();

        // Iterate query results
        foreach (Product item in response)
        {
            Console.WriteLine($"Found item:\t{item.Name}");
        }
    }

    IOrderedQueryable<Product> queryable = container.GetItemLinqQueryable<Product>();

    // Construct LINQ query
    IQueryable<Product> matches = queryable
        .Where(p => p.Category == "gear-surf-surfboards")
        .Where(p => p.Sale == false)
        .Where(p => p.Quantity > 10);

    // Convert to feed iterator
    using FeedIterator<Product> linqFeed = matches.ToFeedIterator();

    // Iterate query result pages
    while (linqFeed.HasMoreResults)
    {
        FeedResponse<Product> response = await linqFeed.ReadNextAsync();

        // Iterate query results
        foreach (Product item in response)
        {
            Console.WriteLine($"Matched item:\t{item.Name}");
        }
    }
    // </query_items_queryable>

}

public record Product(
    string Id,
    string Category,
    string Name,
    int Quantity,
    bool Sale
);