// <using_directives> 
using Microsoft.Azure.Cosmos;
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

}

public record Product(
    string Id,
    string Category,
    string Name,
    int Quantity,
    bool Sale
);