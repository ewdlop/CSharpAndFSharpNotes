using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClassLibrary.Algebra.One.String
{

    public class StringMonoid : StringSemigroup, IMonoid<string>
    {
        public string Identity
        {
            get { return string.Empty; }
        }
    }
}
