using BRICOMA.ECOMMERCE.Data.ApplicationUser;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IPermissionBORepository
    {
        Task<List<Permission>> GetAllPermissions();
        Task<List<ApplicationRole>> GetAllRoles();
        Task<ApplicationRole?> GetRoleById(string id);
        Task<ApplicationRole> CreateRole(ApplicationRole role);
        Task UpdateRole(ApplicationRole role);
        Task DeleteRole(ApplicationRole role);
        Task<List<RolePermission>> GetRolePermissions(string roleId);
        Task SetRolePermissions(string roleId, List<int> permissionIds);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<List<ApplicationUser>> GetUsersPage(int page, int pageSize, string? search, string? roleFilter);
        Task<int> CountUsers(string? search, string? roleFilter);
        Task<Dictionary<string, string>> GetRolesForUsers(List<string> userIds);
    }
}
