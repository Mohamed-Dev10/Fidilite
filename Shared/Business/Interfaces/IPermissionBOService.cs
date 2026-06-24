using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IPermissionBOService
    {
        Task<RESTServiceResponse<List<Permission>>> GetAllPermissions();
        Task<RESTServiceResponse<List<ApplicationRole>>> GetAllRoles();
        Task<RESTServiceResponse<bool>> CreateRole(string name);
        Task<RESTServiceResponse<bool>> UpdateRole(string id, string name);
        Task<RESTServiceResponse<bool>> DeleteRole(string id);
        Task<RESTServiceResponse<List<int>>> GetRolePermissions(string roleId);
        Task<RESTServiceResponse<bool>> SetRolePermissions(string roleId, List<int> permissionIds);
        Task<RESTServiceResponse<List<ApplicationUser>>> GetAllUsers();
        Task<RESTServiceResponse<bool>> CreateUser(CreateUserModel model);
        Task<RESTServiceResponse<bool>> UpdateUser(UpdateUserModel model);
        Task<RESTServiceResponse<bool>> DeleteUser(string id);
        Task<RESTServiceResponse<bool>> ToggleUserSuspension(string id);
    }
}
