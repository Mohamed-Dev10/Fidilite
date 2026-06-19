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
    }
}
