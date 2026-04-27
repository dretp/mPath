using Npgsql;
namespace mPath.Interface;

public interface IMPathHealthPostgresDatabase
{
    NpgsqlCommand GetCommand(NpgsqlDataSource dataSource, string sql);
    NpgsqlDataSource GetDataSource();
}