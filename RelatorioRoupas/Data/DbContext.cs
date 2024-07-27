using Npgsql;
using System.Data;
using System.Data.Common;

namespace RelatorioRoupas.Data
{
    public class DbContext 
    {
        private readonly string _connectionString;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

    }
}
