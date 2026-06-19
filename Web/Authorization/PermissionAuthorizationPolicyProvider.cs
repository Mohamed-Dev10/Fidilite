using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BRICOMA.ECOMMERCE.Web.Authorization
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var existing = await base.GetPolicyAsync(policyName);
            if (existing != null) return existing;

            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("perm", policyName)
                .Build();
        }
    }
}
