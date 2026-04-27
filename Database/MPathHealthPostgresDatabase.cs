using mPath;
using mPath.Interface;
using mPath.Models.Configuration;

using Npgsql;

namespace mPath.Database
{
    public class MPathHealthPostgresDatabase:IMPathHealthPostgresDatabase
    {
        private MpathConfiguration? Config { get; } = MpathConfiguration.Instance;
        public NpgsqlCommand GetCommand(NpgsqlDataSource dataSource, string sql)
        {
            return dataSource.CreateCommand(sql);
        }

        public NpgsqlDataSource GetDataSource()
        {
            
            if (Config?.DbConnectionString != null) return NpgsqlDataSource.Create(Config?.DbConnectionString);
            throw new Exception("No connection string configured.");
        }   

    }
}