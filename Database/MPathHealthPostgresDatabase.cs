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
            var raw = Config?.DbConnectionString?.Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(raw))
            {
                throw new Exception("No DB connection string configured. Set DBConnection environment variable.");
            }

            // Render and some providers expose postgres URLs like:
            // postgres://user:pass@host:5432/db?sslmode=require
            if (raw.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
                || raw.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(raw);
                var userInfo = uri.UserInfo.Split(':', 2);
                var username = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : string.Empty;
                var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;

                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port > 0 ? uri.Port : 5432,
                    Database = uri.AbsolutePath.Trim('/'),
                    Username = username,
                    Password = password,
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true
                };

                return NpgsqlDataSource.Create(builder.ConnectionString);
            }

            return NpgsqlDataSource.Create(raw);
        }   

    }
}