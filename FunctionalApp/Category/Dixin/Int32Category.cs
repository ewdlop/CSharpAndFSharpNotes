using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSharpClassLibrary.Category.Dixin
{
    public class Int32Category : ICategory<int, BinaryExpression>
    {
        public IEnumerable<int> Objects
        {
            get
            {
                for (int int32 = int.MinValue; int32 <= int.MaxValue; int32++)
                {
                    yield return int32;
                }
            }
        }

        public BinaryExpression Compose(BinaryExpression morphism2, BinaryExpression morphism1) =>
            Expression.LessThanOrEqual(morphism2.Left, morphism1.Right); // (Y <= Z) ∘ (X <= Y) => X <= Z.

        public BinaryExpression Id(int @object) =>
            Expression.LessThanOrEqual(Expression.Constant(@object), Expression.Constant(@object)); // X <= X.
    }
}