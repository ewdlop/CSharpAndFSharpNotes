using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CSharpClassLibrary.Service
{
    public interface IDataReaderParser
    {
        public void Parse(IDataReader reader);
    }
}
