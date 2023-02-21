using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity.Core
{
    public class CustomProfileService : ProfileService<ApplicationUser>
    {
        public CustomProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
            : base(userManager, claimsFactory)
        {
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var requestedClaimTypes = context.RequestedClaimTypes;
            var user = context.Subject;
           
            // your implementation to retrieve the requested information
            var claims = GetRequestedClaims(user, requestedClaimTypes);
            context.IssuedClaims.AddRange(claims);

            await base.GetProfileDataAsync(context);
        }

        private IEnumerable<Claim> GetRequestedClaims(ClaimsPrincipal user, IEnumerable<string> requestedClaimsTypes)
        {
            var claims = new List<Claim> { };

            return claims;
        }

        //protected override Task GetProfileDataAsync(ProfileDataRequestContext context)
        //{
        //    var requestedClaimTypes = context.RequestedClaimTypes;
        //    var user = context.Subject;

        //    // your implementation to retrieve the requested information

        //    //var claims = GetRequestedClaims(user, requestedClaimTypes);
        //    //context.IssuedClaims.AddRange(claims);

        //    return base.GetProfileDataAsync(context);
        //}


        //protected override Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
        //{
        //    //var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
        //   // context.IssuedClaims.AddRange(roleClaims);
        //    return base.GetProfileDataAsync(context, user);
        //}

        //protected override async Task<ClaimsPrincipal> GetUserClaimsAsync(ApplicationUser user)
        //{
        //    var claims = await base.GetUserClaimsAsync(user);
        //    return claims;
        //}
    }
}
