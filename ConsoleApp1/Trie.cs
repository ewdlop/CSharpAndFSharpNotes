using BenchmarkDotNet.Attributes;

namespace BenchmarkApp;

[InProcessEmitConfig6]
[MemoryDiagnoser]
public class TrieTest
{
    string[] words = Array.Empty<string>();
    [GlobalSetup]
    public void Setup()
    {
        words = Words.RandonWords;
    }
    
    [Benchmark]
    public void Trie()
    {
        Trie<int> trie = new Trie<int>(77, '.');
        for (int i = 0; i < words.Length; i++)
        {
            trie.InsertWord(words[i], i);
        }
    }

    [Benchmark]
    public void DAFSA()
    {
        DAFSA<int> dAFSA = new DAFSA<int>();
        for (int i = 0; i < words.Length; i++)
        {
            dAFSA.InsertWord(words[i], i);
        }
    }
        
}

public class TrieTest2
{
    private string[] words = Array.Empty<string>();
    private Trie<int> trie;
    private DAFSA<int> dAFSA;
    
    [GlobalSetup]
    public void Setup()
    {
        words = Words.RandonWords;
        trie = new Trie<int>(77, '.');
        for (int i = 0; i < words.Length; i++)
        {
            trie.InsertWord(words[i], i);
        }
        dAFSA = new DAFSA<int>();
        for (int i = 0; i < words.Length; i++)
        {
            dAFSA.InsertWord(words[i], i);
        }
    }

    [Benchmark]
    public void Trie()
    {
        trie.SearchHierarchicalNodes("boundary", (node, indexChar) => node.IsWord || indexChar == '.');
        trie.SearchHierarchicalNodes("breakfast",(node, indexChar) => node.IsWord || indexChar == '.');
        trie.SearchHierarchicalNodes("ancestor", (node, indexChar) => node.IsWord || indexChar == '.');
    }

    [Benchmark]
    public void DAFSA()
    {
        dAFSA.SearchHierarchicalNodes("boundary",(node, indexChar) => node.IsWord || indexChar == '.');
        dAFSA.SearchHierarchicalNodes("breakfast", (node, indexChar) => node.IsWord || indexChar == '.');
        dAFSA.SearchHierarchicalNodes("ancestor", (node, indexChar) => node.IsWord || indexChar == '.');
    }

}
public record Trie<T>(TrieNode<T> Root, int Size, char StartChar)
{
    public Trie(int size, char startChar) : this(new TrieNode<T>(size), size, startChar) { }

    /// <summary>
    /// Return inserted or modified
    /// </summary>
    /// <param name="word"></param>
    /// <param name="obj"></param>
    /// <param name="isWord"></param>
    /// <param name="isPath"></param>
    /// <returns></returns>
    public void InsertWord(string word, T? obj, bool isWord = true, bool isPath = false, string subKey = "")
    {
        TrieNode<T> ws = Root;
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            if (ws.Children.Value[c - StartChar] is null)
            {
                ws.Children.Value[c - StartChar] = new TrieNode<T>(Size);
            }
            ws = ws.Children.Value[c - StartChar];
        }
        ws.IsWord = isWord;
        ws.Value = obj;
        ws.IsPath = isPath;
        ws.SubKey = subKey;
    }

    public (bool Found, T? Result) SearchValue(string word)
    {
        TrieNode<T> ws = Root;
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            if (ws.Children.Value[c - StartChar] is null)
            {
                ws = ws.Children.Value[c - StartChar];
            }
            else
            {
                return (false, default(T));
            }
        }
        if (ws.IsWord)
        {
            return (true, ws.Value);
        }
        else
        {
            return (false, default(T));
        }
    }

    public (bool Found, List<TrieNode<T>?>? Result) SearchHierarchicalNodes(string word, Func<TrieNode<T>, char, bool> matchingCriteria)
    {
        List<TrieNode<T>?>? result = null;
        TrieNode<T> ws = Root;
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            int index = c - StartChar;

            // Out of array bound check
            if (index < 0 || index >= ws.Children.Value.Length)
            {
                return (false, null);
            }
            if (ws.Children.Value[c - StartChar] is null)
            {
                return (false, null);
            }
            else
            {
                ws = ws.Children.Value[c - StartChar];
                if (matchingCriteria(ws, c))
                {
                    (result ??= new List<TrieNode<T>?>()).Add(ws);
                }
            }
        }
        return (result?.Any() ?? false, result);
    }
}
public record TrieNode<T>
{
    public bool IsWord { get; set; }
    public bool IsPath { get; set; }
    public T? Value { get; set; }
    public readonly Lazy<TrieNode<T>?[]> Children;
    public string SubKey { get; set; } = string.Empty;

    public TrieNode(int size)
    {
        Children = new Lazy<TrieNode<T>?[]>(() => new TrieNode<T>?[size]);
    }
}