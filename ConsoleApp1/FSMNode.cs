namespace BenchmarkApp;

// Define the FSM node class
public record FSMNode<T>
{
    public bool IsWord { get; set; }
    public bool IsPath { get; set; }
    public T? Value { get; set; }
    public Dictionary<char, FSMNode<T>> Transitions { get; }
    public string SubKey { get; set; } = string.Empty;
    public FSMNode()
    {
        Transitions = new Dictionary<char, FSMNode<T>>();
    }
}

