using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity.Core
{
    public class ClaimsFactory<T> : UserClaimsPrincipalFactory<T>
        where T : IdentityUser
    {
        private readonly UserManager<T> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimsFactory(
            UserManager<T> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(T user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = roles.SelectMany(role=> _roleManager.GetClaimsAsync(new IdentityRole(role)).Result) ;

            identity.AddClaims(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
            identity.AddClaims(rolesClaims);

            return identity;
        }
    }
}
