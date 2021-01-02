using System.Collections.Generic;

namespace CSharpClassLibrary.Service
{
    public interface ISqlClientService<T>
    {
        public IAsyncEnumerable<ICollection<T>> ReadBatchAsync(int pageSize, int pageSkip = 0, bool GetOneResult = true);
    }
}
