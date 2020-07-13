<Query Kind="Statements" />

await foreach(var dataPoint in FetchIOTData())
{
    dataPoint.Dump();
}

static async IAsyncEnumerable<int> FetchIOTData()
{
	int i = 0;
	while(true)
   {
        await System.Threading.Tasks.Task.Delay(1000);
		i++;
        yield return default(int);
   }
}