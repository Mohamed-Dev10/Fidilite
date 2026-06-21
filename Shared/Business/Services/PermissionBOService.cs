using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BRICOMA.ECOMMERCE.Business.Services
{
    public class PermissionBOService : IPermissionBOService
    {
        private readonly IPermissionBORepository _permissionBORepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PermissionBOService> _logger;

        public PermissionBOService(IPermissionBORepository permissionBORepository, UserManager<ApplicationUser> userManager, ILogger<PermissionBOService> logger)
        {
            _permissionBORepository = permissionBORepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<RESTServiceResponse<List<Permission>>> GetAllPermissions()
        {
            try
            {
                var list = await _permissionBORepository.GetAllPermissions();
                return new RESTServiceResponse<List<Permission>>(true, "OK", list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAllPermissions");
                return new RESTServiceResponse<List<Permission>>(false, ex.Message, new List<Permission>());
            }
        }

        public async Task<RESTServiceResponse<List<ApplicationRole>>> GetAllRoles()
        {
            try
            {
                var list = await _permissionBORepository.GetAllRoles();
                return new RESTServiceResponse<List<ApplicationRole>>(true, "OK", list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAllRoles");
                return new RESTServiceResponse<List<ApplicationRole>>(false, ex.Message, new List<ApplicationRole>());
            }
        }

        public async Task<RESTServiceResponse<bool>> CreateRole(string name, string? description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return new RESTServiceResponse<bool>(false, "Le nom du rôle est obligatoire.", false);

                var role = new ApplicationRole
                {
                    Name = name.Trim(),
                    NormalizedName = name.Trim().ToUpper(),
                    Description = description
                };

                await _permissionBORepository.CreateRole(role);
                _logger.LogInformation("Rôle créé : {Name}", name);
                return new RESTServiceResponse<bool>(true, "Rôle créé avec succès.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur CreateRole - Name: {Name}", name);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> UpdateRole(string id, string name, string? description)
        {
            try
            {
                var role = await _permissionBORepository.GetRoleById(id);
                if (role == null)
                    return new RESTServiceResponse<bool>(false, "Rôle introuvable.", false);

                role.Name = name.Trim();
                role.NormalizedName = name.Trim().ToUpper();
                role.Description = description;

                await _permissionBORepository.UpdateRole(role);
                _logger.LogInformation("Rôle modifié : {Id}", id);
                return new RESTServiceResponse<bool>(true, "Rôle modifié avec succès.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur UpdateRole - Id: {Id}", id);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> DeleteRole(string id)
        {
            try
            {
                var role = await _permissionBORepository.GetRoleById(id);
                if (role == null)
                    return new RESTServiceResponse<bool>(false, "Rôle introuvable.", false);

                await _permissionBORepository.DeleteRole(role);
                _logger.LogInformation("Rôle supprimé : {Id}", id);
                return new RESTServiceResponse<bool>(true, "Rôle supprimé.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur DeleteRole - Id: {Id}", id);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<List<int>>> GetRolePermissions(string roleId)
        {
            try
            {
                var rp = await _permissionBORepository.GetRolePermissions(roleId);
                var ids = rp.Select(r => r.PermissionId).ToList();
                return new RESTServiceResponse<List<int>>(true, "OK", ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetRolePermissions - RoleId: {RoleId}", roleId);
                return new RESTServiceResponse<List<int>>(false, ex.Message, new List<int>());
            }
        }

        public async Task<RESTServiceResponse<bool>> SetRolePermissions(string roleId, List<int> permissionIds)
        {
            try
            {
                var role = await _permissionBORepository.GetRoleById(roleId);
                if (role == null)
                    return new RESTServiceResponse<bool>(false, "Rôle introuvable.", false);

                await _permissionBORepository.SetRolePermissions(roleId, permissionIds);
                _logger.LogInformation("Permissions mises à jour pour le rôle : {RoleId}", roleId);
                return new RESTServiceResponse<bool>(true, "Permissions enregistrées.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur SetRolePermissions - RoleId: {RoleId}", roleId);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<List<ApplicationUser>>> GetAllUsers()
        {
            try
            {
                var list = await _permissionBORepository.GetAllUsers();
                return new RESTServiceResponse<List<ApplicationUser>>(true, "OK", list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAllUsers");
                return new RESTServiceResponse<List<ApplicationUser>>(false, ex.Message, new List<ApplicationUser>());
            }
        }

        public async Task<RESTServiceResponse<bool>> CreateUser(CreateUserModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                    return new RESTServiceResponse<bool>(false, "Email et mot de passe sont obligatoires.", false);

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return new RESTServiceResponse<bool>(false, string.Join(", ", result.Errors.Select(e => e.Description)), false);

                if (!string.IsNullOrWhiteSpace(model.RoleId))
                {
                    var role = await _permissionBORepository.GetRoleById(model.RoleId);
                    if (role?.Name != null)
                        await _userManager.AddToRoleAsync(user, role.Name);
                }

                _logger.LogInformation("Utilisateur créé : {Email}", model.Email);
                return new RESTServiceResponse<bool>(true, "Utilisateur créé avec succès.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur CreateUser - Email: {Email}", model.Email);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> UpdateUser(UpdateUserModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                    return new RESTServiceResponse<bool>(false, "Utilisateur introuvable.", false);

                user.Email = model.Email;
                user.UserName = model.Email;

                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                }

                await _userManager.UpdateAsync(user);

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!string.IsNullOrWhiteSpace(model.RoleId))
                {
                    var role = await _permissionBORepository.GetRoleById(model.RoleId);
                    if (role?.Name != null)
                        await _userManager.AddToRoleAsync(user, role.Name);
                }

                _logger.LogInformation("Utilisateur modifié : {Id}", model.Id);
                return new RESTServiceResponse<bool>(true, "Utilisateur modifié.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur UpdateUser - Id: {Id}", model.Id);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new RESTServiceResponse<bool>(false, "Utilisateur introuvable.", false);

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return new RESTServiceResponse<bool>(false, string.Join(", ", result.Errors.Select(e => e.Description)), false);

                _logger.LogInformation("Utilisateur supprimé : {Id}", id);
                return new RESTServiceResponse<bool>(true, "Utilisateur supprimé.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur DeleteUser - Id: {Id}", id);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> ToggleUserSuspension(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new RESTServiceResponse<bool>(false, "Utilisateur introuvable.", false);

                var isSuspended = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow;

                if (isSuspended)
                {
                    await _userManager.SetLockoutEndDateAsync(user, null);
                    _logger.LogInformation("Compte réactivé : {Id}", id);
                    return new RESTServiceResponse<bool>(true, "Compte réactivé.", true);
                }

                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                // Invalide la session active du compte (déconnexion auto s'il est déjà connecté)
                await _userManager.UpdateSecurityStampAsync(user);
                _logger.LogInformation("Compte suspendu : {Id}", id);
                return new RESTServiceResponse<bool>(true, "Compte suspendu.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur ToggleUserSuspension - Id: {Id}", id);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }
    }
}
