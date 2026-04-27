using mPath.Models.Role;

namespace mPath.Interface;

public interface IRoleService
{
    Task<List<RoleModel>> GetRoles();
    Task<RoleModel?> GetRole(int id);
}
