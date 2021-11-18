namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public interface IVectorSpace<
        TScalar,
        TVector,
        TScalarField,
        TScalarAdditiveAbelianGroup,
        TScalarMultiplicativeAbelianGroup,
        TVectorAdditiveAbelianGroup
    > : IUnitaryModule<
        TScalar,
        TVector,
        TScalarField,
        TScalarAdditiveAbelianGroup,
        TScalarMultiplicativeAbelianGroup,
        TVectorAdditiveAbelianGroup
    >
        where TScalarField : IField<TScalar, TScalarAdditiveAbelianGroup, TScalarMultiplicativeAbelianGroup>
        where TScalarAdditiveAbelianGroup : IAbelianGroup<TScalar>
        where TScalarMultiplicativeAbelianGroup : IAbelianGroup<TScalar>
        where TVectorAdditiveAbelianGroup : IAbelianGroup<TVector>
    {

    }
}
