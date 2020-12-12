using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CSharpClassLibrary.Service
{
    public interface ISqlClientService<T>
    {
        public IAsyncEnumerable<ICollection<T>> ReadBatchAsync(int pageSize, int pageSkip = 0);
    }
}
