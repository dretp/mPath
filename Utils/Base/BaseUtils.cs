using mPath.Database;
using mPath.Interface;
using Npgsql;
using mPath.Middleware;
namespace mPath.Utils.Base;

public partial class BaseUtils
{
    private readonly IMPathHealthPostgresDatabase _db;
    
    
    public BaseUtils()
    {
        _db = new MPathHealthPostgresDatabase();
        
    }
    

    protected NpgsqlDataSource dataSource()
    {
        return _db.GetDataSource();
    }

    protected NpgsqlCommand command(NpgsqlDataSource dataSource, string sql)
    {
        return _db.GetCommand(dataSource, sql);
    }
    
    protected void Log(string msg)
    {
        Console.WriteLine(msg);
    }

    protected void LogError(Exception ex, string util)
    {
        Console.WriteLine($" Error in {util} {ex.Message} - {ex.StackTrace}");
    }

    protected string createDisplayName(string fname, string lname)
    {
        return string.Concat(fname, " ", lname[..1], ".");
    }
}