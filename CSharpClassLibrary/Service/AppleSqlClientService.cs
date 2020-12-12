using CSharpClassLibrary.Service.Model;

namespace CSharpClassLibrary.Service
{
    public class AppleSqlClientService : SqlClientService<Apple>
    {
        public AppleSqlClientService(string connectionString) : base(connectionString) {
        }
    }
}
