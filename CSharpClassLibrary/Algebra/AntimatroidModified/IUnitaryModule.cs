namespace CSharpClassLibrary.Algebra.One
{
    public interface IUnitaryModule<
        TScalar,
        TVector,
        TScalarRingWithUnity,
        TScalarAdditiveGroup,
        TScalarMultiplicativeMonoid,
        TVectorAdditiveAbelianGroup
    > : IModule<
        TScalar,
        TVector,
        TScalarRingWithUnity,
        TScalarAdditiveGroup,
        TScalarMultiplicativeMonoid,
        TVectorAdditiveAbelianGroup
    >
        where TScalarRingWithUnity : IRingWithUnity<TScalar, TScalarAdditiveGroup, TScalarMultiplicativeMonoid>
        where TScalarAdditiveGroup : IGroup<TScalar>
        where TScalarMultiplicativeMonoid : IMonoid<TScalar>
        where TVectorAdditiveAbelianGroup : IAbelianGroup<TVector>
    {

    }
}
