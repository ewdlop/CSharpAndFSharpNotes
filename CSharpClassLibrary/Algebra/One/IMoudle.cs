namespace CSharpClassLibrary.Algebra.One
{
    public interface IMoudle<
        TScalar,
        TVector,
        TScalarRing,
        TScalarAdditiveGroup,
        TScalarMultiplicativeMonoid,
        TVectorAdditiveAbelieanGroup>
        where TScalarRing : IRing<TScalar, TScalarAdditiveGroup, TScalarMultiplicativeMonoid>
        where TScalarAdditiveGroup : IGroup<TScalar>
        where TScalarMultiplicativeMonoid : IMonoid<TScalar>
        where TVectorAdditiveAbelieanGroup : IAbelianGroup<TVector>
    {
        TScalarRing Scalar { get; }
        TVectorAdditiveAbelieanGroup Vector { get; }
        TVector Distribute(TScalar t, TVector r);
    }
}
