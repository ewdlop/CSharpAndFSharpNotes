using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public
{
    public partial class Husband
    {
        private class Wife
        {
            public int GetAge()
            {
                throw new Exception("yo");
            }
        }
    }
}