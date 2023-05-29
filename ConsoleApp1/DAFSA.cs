namespace BenchmarkApp;

//DAFSA,deterministic acyclic finite state automaton 
public record DAFSA<T>
{
    private readonly FSMNode<T> root;

    public DAFSA()
    {
        root = new FSMNode<T>();
    }

    // Insert a word into the AFSM
    public void InsertWord(string word, T obj, bool isWord = true, bool isPath = false, string subKey = "")
    {
        FSMNode<T> node = root;
        foreach (char c in word)
        {
            if (!node.Transitions.TryGetValue(c, out FSMNode<T>? nextNode))
            {
                nextNode = new FSMNode<T>();
                node.Transitions.Add(c, nextNode);
            }
            node = nextNode;
        }
        node.IsWord = isWord;
        node.Value = obj;
        node.IsPath = isPath;
        node.SubKey = subKey;
    }

    // Perform a search operation to check if a word is recognized by the AFSM
    public (bool Found, T? Result) Search(string word)
    {
        FSMNode<T> node = root;
        foreach (char c in word)
        {
            if (!node.Transitions.TryGetValue(c, out FSMNode<T>? nextNode))
            {
                return (false, default(T)); // Transition doesn't exist, word not recognized
            }
            node = nextNode;
        }
        if (node.IsWord)
        {
            return (true, node.Value);
        }
        else
        {
            return (false, default(T));
        }
    }

    public (bool Found, List<FSMNode<T>?>? Result) SearchHierarchicalNodes(string word, Func<FSMNode<T>, char, bool> matchingCriteria)
    {
        List<FSMNode<T>?>? result = null;
        FSMNode<T> node = root;
        foreach (char c in word)
        {
            if (node.Transitions.TryGetValue(c, out FSMNode<T>? nextNode))
            {
                if (matchingCriteria(node, c))
                {
                    (result ??= new List<FSMNode<T>?>()).Add(node);
                }
                node = nextNode;
            }
        }
        return (result?.Any() ?? false, result);
    }
}

