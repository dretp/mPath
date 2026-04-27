using mPath.Interface;
using mPath.Models.Role;
using mPath.Utils.Role;

namespace mPath.Services;

public class RoleService(RoleUtil roleUtil) : IRoleService
{
    private readonly RoleUtil _roleUtil = roleUtil;

    public async Task<List<RoleModel>> GetRoles()
    {
        return await _roleUtil.GetRoles();
    }

    public async Task<RoleModel?> GetRole(int id)
    {
        return await _roleUtil.GetRoleById(id);
    }
}
