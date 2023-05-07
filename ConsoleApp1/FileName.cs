using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace BenchmarkApp;
public class InProcessEmitConfig6Attribute : ConfigAttribute
{
    public InProcessEmitConfig6Attribute() : base(typeof(InProcessEmit6Config))
    {
    }
}

public class InProcessEmitConfig7Attribute : ConfigAttribute
{
    public InProcessEmitConfig7Attribute() : base(typeof(InProcessEmit7Config))
    {
    }
}

public class InProcessEmit6Config : ManualConfig
{
    public InProcessEmit6Config()
    {
        AddJob(Job.Default
            .WithToolchain(InProcessEmitToolchain.Instance)
            .WithRuntime(CoreRuntime.Core60));
    }
}

public class InProcessEmit7Config : ManualConfig
{
    public InProcessEmit7Config()
    {
        AddJob(Job.Default
            .WithToolchain(InProcessEmitToolchain.Instance)
            .WithRuntime(CoreRuntime.Core70));
    }
}

[InProcessEmitConfig6]
[MemoryDiagnoser]
public class ReadOnlyListRecordClass
{
    //[Benchmark]
    //public void ReadOnlyListRecordArray()
    //{
    //    for(int i = 0; i < 1000; i++)
    //    {
    //        ReadOnlyListRecord<int> x = new ReadOnlyListRecord<int>(new int[] { 1, 2, 3, 4, 5 });
    //    }
    //}

    [Benchmark]
    public void ReadOnlyListRecord2Array()
    {
        for (int i = 0; i < 1000; i++)
        {
            ReadOnlyListRecord2<int> x = new int[] { 1, 2, 3, 4, 5 };
        }
    }


    [Benchmark]
    public void ReadOnlyListRecord3Array()
    {
        for (int i = 0; i < 1000; i++)
        {
            ReadOnlyListRecord3<int> x = new int[] { 1, 2, 3, 4, 5 };
        }
    }

    //[Benchmark]
    //public void ReadOnlyListRecordList()
    //{
    //    for (int i = 0; i < 1000; i++)
    //    {
    //        ReadOnlyListRecord<int> x = new ReadOnlyListRecord<int>(new List<int> { 1, 2, 3, 4, 5 });
    //    }
    //}
    
    [Benchmark]
    public void ReadOnlyListRecord2List()
    {
        for (int i = 0; i < 1000; i++)
        {
            ReadOnlyListRecord2<int> x = new ReadOnlyListRecord2<int>(new List<int> { 1, 2, 3, 4, 5 });
        }
    }

    [Benchmark]
    public void ReadOnlyListRecord3List()
    {
        for (int i = 0; i < 1000; i++)
        {
            ReadOnlyListRecord3<int> x = new ReadOnlyListRecord3<int>(new List<int> { 1, 2, 3, 4, 5 });
        }
    }

    //[Benchmark]
    //public void ReadOnlyListRecordTestObject()
    //{
    //    for (int i = 0; i < 1000; i++)
    //    {
    //        ReadOnlyListRecord<TestObject> x = new ReadOnlyListRecord<TestObject>(new TestObject[] { new TestObject(), new TestObject() , new TestObject() , new TestObject() , new TestObject() });
    //    }
    //}

    [Benchmark]
    public void ReadOnlyListRecord2TestObject()
    {
        for (int i = 0; i < 1000; i++)
        {
            ReadOnlyListRecord2<TestObject> x = new ReadOnlyListRecord2<TestObject>(new TestObject[] { new TestObject(), new TestObject(), new TestObject(), new TestObject(), new TestObject() });
        }
    }

    [Benchmark]
    public void ReadOnlyListRecord3TestObject()
    {
        for (int i = 0; i < 1000; i++)
        {
            ReadOnlyListRecord3<TestObject> x = new ReadOnlyListRecord3<TestObject>(new TestObject[] { new TestObject(), new TestObject(), new TestObject(), new TestObject(), new TestObject() });
        }
    }
}

public class TestObject
{

}

[Serializable]
public sealed record ReadOnlyListRecord2<T> : IReadOnlyList<T>
{
    public static implicit operator ReadOnlyListRecord2<T>(List<T> input) => new ReadOnlyListRecord2<T>(input.AsEnumerable());
    public static implicit operator ReadOnlyListRecord2<T>(T[] input) => new ReadOnlyListRecord2<T>(input.AsEnumerable());
    public static implicit operator ReadOnlyListRecord2<T>(T input) => new ReadOnlyListRecord2<T>(input);
    public static readonly ReadOnlyListRecord2<T> Empty = new ReadOnlyListRecord2<T>();
    private readonly IReadOnlyList<T> _baseList;
    public ReadOnlyListRecord2()
    {
        _baseList = Array.Empty<T>();
    }
    public ReadOnlyListRecord2(IEnumerable<T> items)
    {
        _baseList = items.ToList() ?? throw new ArgumentNullException(nameof(items));
    }
    public ReadOnlyListRecord2(T input)
    {
        _baseList = new List<T>() { input };
    }
    /// <summary>
    /// Gets or initializes the items in this collection.
    /// </summary>
    public IReadOnlyCollection<T> Items => _baseList;

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    public bool Contains(T item) => Items.Contains(item);

    //Has intersect
    public bool ContainsAny(IEnumerable<T> items) => Items.Intersect(items).Any();

    //ToHashSet
    public HashSet<T> ToHashSet() => new HashSet<T>(Items);

    /// <inheritdoc />
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return (Items as System.Collections.IEnumerable).GetEnumerator();
    }
    /// <inheritdoc />
    //[IgnoreMember]
    public int Count => _baseList.Count;

    /// <inheritdoc />
    public T this[int index] => _baseList[index];

    /// <inheritdoc />
    public override int GetHashCode()
    {
        int someHashValue = -234897289;
        for (int i = 0; i < _baseList.Count; i++)
        {
            T? item = _baseList[i];
            someHashValue = someHashValue ^ item?.GetHashCode() ?? int.MinValue;
        }

        return someHashValue;
    }
    private static bool DefaultValueEquals(T x, T y) =>
        object.ReferenceEquals(x, y)
        || (x as IEquatable<T>)?.Equals(y) == true
        || object.Equals(x, y);

    /// <inheritdoc />
    public bool Equals(ReadOnlyListRecord2<T>? other)
    {
        //The contence are immutable too
        if (ReferenceEquals(this, other)) return true;

        // create a proper equality method...
        if (other == null || other.Count != Count)
        {
            return false;
        }
        for (int i = 0; i < Count; i++)
        {
            if (!DefaultValueEquals(this[i], other[i]))
            {
                return false;
            }
        }
        return true;
    }
}


public sealed record ReadOnlyListRecord3<T> : IReadOnlyList<T>
{
    public static implicit operator ReadOnlyListRecord3<T>(List<T> input) => new ReadOnlyListRecord3<T>(input.AsEnumerable());
    public static implicit operator ReadOnlyListRecord3<T>(T[] input) => new ReadOnlyListRecord3<T>(input.AsEnumerable());
    public static implicit operator ReadOnlyListRecord3<T>(T input) => new ReadOnlyListRecord3<T>(input);
    public static readonly ReadOnlyListRecord3<T> Empty = new ReadOnlyListRecord3<T>();
    private readonly IReadOnlyList<T> _baseList;
    public ReadOnlyListRecord3()
    {
        _baseList = Array.Empty<T>();
    }
    public ReadOnlyListRecord3(IEnumerable<T> items)
    {
        _baseList = items.ToArray() ?? throw new ArgumentNullException(nameof(items));
    }
    public ReadOnlyListRecord3(T input)
    {
        _baseList = new T[1]{ input };
    }
    /// <summary>
    /// Gets or initializes the items in this collection.
    /// </summary>
    public IReadOnlyCollection<T> Items => _baseList;

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    public bool Contains(T item) => Items.Contains(item);

    //Has intersect
    public bool ContainsAny(IEnumerable<T> items) => Items.Intersect(items).Any();

    //ToHashSet
    public HashSet<T> ToHashSet() => new HashSet<T>(Items);

    /// <inheritdoc />
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return (Items as System.Collections.IEnumerable).GetEnumerator();
    }
    /// <inheritdoc />
    //[IgnoreMember]
    public int Count => _baseList.Count;

    /// <inheritdoc />
    public T this[int index] => _baseList[index];

    /// <inheritdoc />
    public override int GetHashCode()
    {
        int someHashValue = -234897289;
        for (int i = 0; i < _baseList.Count; i++)
        {
            T? item = _baseList[i];
            someHashValue = someHashValue ^ item?.GetHashCode() ?? int.MinValue;
        }

        return someHashValue;
    }
    private static bool DefaultValueEquals(T x, T y) =>
        object.ReferenceEquals(x, y)
        || (x as IEquatable<T>)?.Equals(y) == true
        || object.Equals(x, y);

    /// <inheritdoc />
    public bool Equals(ReadOnlyListRecord3<T>? other)
    {
        //The contence are immutable too
        if (ReferenceEquals(this, other)) return true;

        // create a proper equality method...
        if (other == null || other.Count != Count)
        {
            return false;
        }
        for (int i = 0; i < Count; i++)
        {
            if (!DefaultValueEquals(this[i], other[i]))
            {
                return false;
            }
        }
        return true;
    }
}
public static class ReadOnlyListRecord2Extensions
{
    public static ReadOnlyListRecord2<T> ToReadOnlyListRecord2<T>(this IEnumerable<T> items)
    {
        return new ReadOnlyListRecord2<T>(items);
    }
}
