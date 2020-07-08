<Query Kind="Program" />

void Main()
{
	var simpleUri = GetSimpleUri();
	simpleUri.Dump();
}

// Define other methods, classes and namespaces here
Uri GetSimpleUri()
{
	var builder = new UriBuilder {
		Scheme = "http",
		Host = "packt.com"
	};
	return builder.Uri;
}