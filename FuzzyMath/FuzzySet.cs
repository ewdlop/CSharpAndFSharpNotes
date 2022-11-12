using System.Numerics;

namespace FuzzyMath;

public record FuzzySet<T1,T2> where T1: INumber<T1>
    where T2: INumber<T2>
{
    public required HashSet<T1> BaseSet { get; init; }
    /// <summary>
    /// T -> [0,1]
    /// </summary>
    public required Func<T1, T2> MemberShip { get; init; }
    public required Func<T2, T2> Negator { get; set; } 
    public required Func<T2, T2, T2> TNorm { get; init; }
    public FuzzySet<T1, T2> ToComplement => this with
    {
        MemberShip = element => Negator(MemberShip(element))
    };
    public FuzzySet<T1, T2> ToIntersection(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => TNorm(MemberShip(element), set.MemberShip(element))
    };
    public FuzzySet<T1, T2> ToUnion(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => SNorm(MemberShip(element), set.MemberShip(element))
    };
    public FuzzySet<T1, T2> ToPower(T2 power)
    {
        if (power <= T2.Zero) throw new NotDefinedException();
        throw new NotImplementedException();
    }
    public FuzzySet<T1, T2> ToConcentration() => this with
    {
        MemberShip = element => MemberShip(element) * MemberShip(element)
    };
    public FuzzySet<T1, T2> ToDifference1(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => TNorm(MemberShip(element), Negator(set.MemberShip(element)))
    };
    public FuzzySet<T1, T2> ToDifference2(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => T2.Min(MemberShip(element), T2.One - set.MemberShip(element))
    };
    public FuzzySet<T1, T2> ToDifference3(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => MemberShip(element) - TNorm(MemberShip(element), set.MemberShip(element))
    };
    public FuzzySet<T1, T2> ToSymmetric1(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => T2.Abs(MemberShip(element) - set.MemberShip(element))
    };
    public FuzzySet<T1, T2> ToSymmetric2(FuzzySet<T1, T2> set) => this with
    {
        MemberShip = element => T2.Max(T2.Min(MemberShip(element),T2.One - set.MemberShip(element)), T2.Min(set.MemberShip(element), T2.One - MemberShip(element)))
    };

    public Func<T1, T2> NormalizedMemberShip => e => MemberShip(e) / Height;
    public Func<T2, T2, T2> SNorm => (m1, m2) => T2.One - TNorm(T2.One - m1, T2.One - m2);
    //public T2 Cardinality() => BaseSet.Sum(e=> MemberShip(e))
    public T2 Cardinality(Func<T1, T2>? memberShip = null)
    {
        T2 sum = T2.AdditiveIdentity;
        foreach(T1 element in BaseSet)
        {
            sum += (memberShip ??= MemberShip)(element);
        }
        return sum;
    }
    public T2 RelativeCardinality() => Cardinality() / T2.CreateChecked(BaseSet.Count);
    public T2 RelativeCardinality(FuzzySet<T1, T2> set) => Cardinality(element => TNorm(MemberShip(element), set.MemberShip(element))) / set.Cardinality();
    public T2 Grade(T1 element) 
    {
        CheckBaseSetMemberShip(element);
        return MemberShip(element);
    }
    public T2 Height => BaseSet.Max(e => MemberShip(e));
    /// <summary>
    /// Cannot guaranteed membership in C#
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public bool NotIncluded(T1 element) => CheckBaseSetMemberShip(element) && MemberShip(element) == T2.Zero;
    public bool FullyIncluded(T1 element) => CheckBaseSetMemberShip(element) && MemberShip(element) == T2.One;
    public bool PartiallyIncluded(T1 element) => CheckBaseSetMemberShip(element) && MemberShip(element) > T2.Zero && MemberShip(element) < T2.One;
    public bool CrossOverPoint(T1 element) => CheckBaseSetMemberShip(element) && MemberShip(element) == T2.CreateChecked(0.5);
    /// <summary>
    /// Crisp Sets
    /// </summary>
    /// <param name="cut"></param>
    /// <returns></returns>
    public IEnumerable<T1> AsAlaphaCut(T2 cut) => BaseSet.Where(e => MemberShip(e) >= cut);
    public IEnumerable<T1> AsStrongAlaphaCut(T2 cut) => BaseSet.Where(e => MemberShip(e) > cut);
    public IEnumerable<T1> AsSupport() => BaseSet.Where(e => MemberShip(e) > T2.Zero);
    public IEnumerable<T1> AsKernel() => BaseSet.Where(e => MemberShip(e) == T2.Zero);
    public IEnumerable<T1> AsCore() => BaseSet.Where(e => MemberShip(e) == T2.Zero);
    /// <summary>
    /// Calcuate distinct cuts, actual defintion is harder to compute
    /// </summary>
    /// <returns></returns>
    public HashSet<T2> LevelSet() => BaseSet.Select(e => MemberShip(e)).ToHashSet();
    public bool IsEmpty => BaseSet.All(e => MemberShip(e) == T2.Zero);
    public bool IsSetEqual(FuzzySet<T1,T2> set) => BaseSet.All(e => MemberShip(e) == set.MemberShip(e));
    public bool IsIncludedIn(FuzzySet<T1, T2> set) => BaseSet.All(e => MemberShip(e) <= set.MemberShip(e));
    public bool IsLevel(T2 cut) => BaseSet.Where(e => MemberShip(e) == cut).Any();
    public bool IsNormalized() => Height == T2.One;
    public bool IsDisjoint(FuzzySet<T1, T2> set) => BaseSet.All(e => T2.Min(MemberShip(e),set.MemberShip(e)) == T2.Zero);
    private bool CheckBaseSetMemberShip(T1 element)
    {
        if (!BaseSet.Contains(element)) throw new NotDefinedException("The element is not found in the base set");
        return true;
    }

    /// <summary>
    /// Possible not works for these. The set needs to be measureable for these
    /// </summary>
    public T1 Width => AsSupport().Max() - AsSupport().Min();
    public bool CheckConvexity(T1 element1, T1 element2, T1 step)
    {
        if (T1.Abs(step) > T1.One) throw new ArgumentOutOfRangeException();
        return CheckBaseSetMemberShip(element1) && CheckBaseSetMemberShip(element2) &&
            MemberShip(step * element1 + (T1.One - step) * element2) >= T2.Min(MemberShip(element1), MemberShip(element2));
    }
    public bool IsConvex()
    {
        //CheckConvexity for all (element1, element2, 0 <= step <= 1)
        throw new NotDefinedException();
    }
}

[Serializable]
public class NotDefinedException : Exception
{
    public NotDefinedException(){ }

    public NotDefinedException(string message): base(message){ }

    public NotDefinedException(string message, Exception innerException): base(message, innerException){ }
}