using BenchmarkApp;
using BenchmarkDotNet.Running;

//BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<LazyTasks>();
//BenchmarkRunner.Run<ReadOnlyListRecordClass>(ManualConfig.CreateMinimumViable()
//    .AddJob(Job.Default
//        .WithToolchain(InProcessEmitToolchain.Instance)
//        .WithRuntime(CoreRuntime.Core60)));

//BenchmarkRunner.Run<ReadOnlyListRecordClass>(); 
//BenchmarkRunner.Run<SplitterTest>();

//Trie<int> trie = new Trie<int>(77, '.');
//for (int i = 0; i < stopWords.Length; i++)
//{
//    trie.InsertWord(stopWords[i], i);
//}

BenchmarkRunner.Run<TrieTest2>();