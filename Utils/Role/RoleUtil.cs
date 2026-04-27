using System.Text;
using mPath.Models.Role;
using mPath.Utils.Base;

namespace mPath.Utils.Role;

public class RoleUtil : BaseUtils
{
    public RoleUtil()
    {
    }

    public async Task<List<RoleModel>> GetRoles()
    {
        return await retrieveRoles();
    }

    public async Task<RoleModel?> GetRoleById(int id)
    {
        return await retrieveRoleById(id);
    }

    private async Task<List<RoleModel>> retrieveRoles()
    {
        var roles = new List<RoleModel>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT id, name FROM roles ORDER BY id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(new RoleModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader["name"].ToString() ?? string.Empty
                });
            }

            return roles;
        }
        catch (Exception e)
        {
            LogError(e, "RoleUtil.retrieveRoles");
            return roles;
        }
    }

    private async Task<RoleModel?> retrieveRoleById(int id)
    {
        var sql = new StringBuilder();
        sql.AppendLine("SELECT id, name FROM roles WHERE id = @id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@id", id);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new RoleModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader["name"].ToString() ?? string.Empty
                };
            }

            return null;
        }
        catch (Exception e)
        {
            LogError(e, "RoleUtil.retrieveRoleById");
            return null;
        }
    }
}
