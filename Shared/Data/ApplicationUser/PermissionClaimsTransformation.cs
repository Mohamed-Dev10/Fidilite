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

            // Pas de cache : on relit TOUJOURS les permissions depuis la DB
            // pour qu'un changement admin s'applique immédiatement.
            var existingPerms = principal.Claims.Where(c => c.Type == "perm").ToList();
            if (existingPerms.Any())
            {
                var oldIdentity = principal.Identities.FirstOrDefault(i => i.Claims.Any(c => c.Type == "perm"));
                if (oldIdentity != null)
                    foreach (var c in existingPerms) oldIdentity.TryRemoveClaim(c);
            }

            // Id lu directement depuis le claim (aucun aller-retour DB), puis UNE SEULE requête
            // jointe (UserRoles → Roles → RolePermissions) au lieu de 3 requêtes séparées.
            // Cette transformation s'exécute sur CHAQUE requête authentifiée (login compris),
            // donc chaque round-trip économisé compte.
            var userId = _userManager.GetUserId(principal);
            if (string.IsNullOrEmpty(userId)) return principal;

            var permissions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Id)
                .Join(_context.RolePermissions, roleId => roleId, rp => rp.RoleId, (roleId, rp) => rp.Permission!.Code)
                .Distinct()
                .ToListAsync();

            var newIdentity = new ClaimsIdentity();
            foreach (var perm in permissions)
                newIdentity.AddClaim(new Claim("perm", perm));

            principal.AddIdentity(newIdentity);
            return principal;
        }
    }
}
