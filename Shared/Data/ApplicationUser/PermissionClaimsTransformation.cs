using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BRICOMA.ECOMMERCE.Data.ApplicationUser
{
    public class PermissionClaimsTransformation : IClaimsTransformation
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionClaimsTransformation(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.IsAuthenticated != true)
                return principal;

            if (principal.HasClaim(c => c.Type == "perm"))
                return principal;

            var user = await _userManager.GetUserAsync(principal);
            if (user == null) return principal;

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any()) return principal;

            var roleIds = await _context.Roles
                .Where(r => roles.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync();

            var permissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.Permission!.Code)
                .Distinct()
                .ToListAsync();

            var identity = new ClaimsIdentity();
            foreach (var perm in permissions)
                identity.AddClaim(new Claim("perm", perm));

            principal.AddIdentity(identity);
            return principal;
        }
    }
}
