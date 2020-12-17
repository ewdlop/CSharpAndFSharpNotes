using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CSharpClassLibrary.Service
{
    public abstract class SqlClientService<T> : ISqlClientService<T> where T : IDataReaderParser, new()
    {
        public string ConnectionString { get; }

        public SqlClientService(string connectionString) {
            ConnectionString = connectionString;
        }

        public async IAsyncEnumerable<ICollection<T>> ReadBatchAsync(int pageSize, int pageSkip = 0) {
            using (var connection = SqlClientFactory.Instance.CreateConnection()) {
                //assume Model name match table name
                string tableName = typeof(T).Name;
                string queryString = $"SELECT * From {tableName} OFFSET @Skip ROWS FETCH Next @Take ROWS ONLY ";

                connection.ConnectionString = ConnectionString;

                DbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                //command.CommandType = CommandType.StoredProcedure;
                //command.CommandText = "sp_name";

                command.Parameters.Add(new SqlParameter("@Take", SqlDbType.Int) {
                    Direction = ParameterDirection.Input,
                    Value = pageSize
                });
                if (pageSkip > 0) {
                    command.Parameters.Add(new SqlParameter("@Skip", SqlDbType.Int) {
                        Direction = ParameterDirection.Input,
                        Value = pageSkip
                    });
                }
                connection.Open();
                using (DbDataReader reader = await command.ExecuteReaderAsync()) {
                    while (reader.NextResult()) {
                        var page = new Collection<T>();
                        while (await reader.ReadAsync()) {
                            var row = new T();
                            row.Parse(reader);
                            page.Add(row);
                        }
                        yield return page;
                    }
                }
            }
            yield return null;
        }
    }
}
