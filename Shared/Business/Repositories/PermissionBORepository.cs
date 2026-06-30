using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Business.Repositories
{
    public class PermissionBORepository : IPermissionBORepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionBORepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllPermissions()
        {
            return await _context.Permissions.OrderBy(p => p.Category).ThenBy(p => p.Label).ToListAsync();
        }

        public async Task<List<ApplicationRole>> GetAllRoles()
        {
            return await _context.Roles.OrderBy(r => r.Name).ToListAsync();
        }

        public async Task<ApplicationRole?> GetRoleById(string id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ApplicationRole> CreateRole(ApplicationRole role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task UpdateRole(ApplicationRole role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRole(ApplicationRole role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RolePermission>> GetRolePermissions(string roleId)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
        }

        public async Task SetRolePermissions(string roleId, List<int> permissionIds)
        {
            var existing = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            _context.RolePermissions.RemoveRange(existing);

            var newEntries = permissionIds.Select(pid => new RolePermission
            {
                RoleId = roleId,
                PermissionId = pid
            }).ToList();

            _context.RolePermissions.AddRange(newEntries);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _context.Users.OrderBy(u => u.UserName).ToListAsync();
        }

        // Sous-requête réutilisée : Id des users ayant le rôle donné (jointure UserRoles + Roles).
        private IQueryable<string> UserIdsInRole(string roleFilter)
        {
            return _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .Where(x => x.Name == roleFilter)
                .Select(x => x.UserId);
        }

        private IQueryable<ApplicationUser> FilteredUsersQuery(string? search, string? roleFilter)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u => u.Email != null && u.Email.Contains(search));

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                var ids = UserIdsInRole(roleFilter);
                query = query.Where(u => ids.Contains(u.Id));
            }

            return query;
        }

        // Pagination SQL (Skip/Take) : seule la page demandée est chargée, pas toute la table.
        public async Task<List<ApplicationUser>> GetUsersPage(int page, int pageSize, string? search, string? roleFilter)
        {
            return await FilteredUsersQuery(search, roleFilter)
                .OrderBy(u => u.Email)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountUsers(string? search, string? roleFilter)
        {
            return await FilteredUsersQuery(search, roleFilter).CountAsync();
        }

        // Charge le rôle de plusieurs users EN UNE SEULE requête (au lieu d'un appel par user).
        public async Task<Dictionary<string, string>> GetRolesForUsers(List<string> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return new Dictionary<string, string>();

            var data = await _context.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, RoleName = r.Name })
                .ToListAsync();

            return data
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.First().RoleName ?? "-");
        }
    }
}
