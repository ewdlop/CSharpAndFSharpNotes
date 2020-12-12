using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CSharpClassLibrary.Service.Model
{

    public class Apple : IDataReaderParser
    {
        public void Parse(IDataReader reader) {
            throw new NotImplementedException();
        }
    }
}
