using System.Security.Claims;
using System.Xml.Linq;
using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServerAspNetIdentity;

public class SeedData
{
    private const string RoleAdmin = "Admin";

    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
            CreateRolesAsync(scope.ServiceProvider).GetAwaiter().GetResult();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            CreateAliceAsync(userMgr).GetAwaiter().GetResult();
            CreateBobAsync(userMgr).GetAwaiter().GetResult();

        }
    }

    private static async Task CreateAliceAsync(UserManager<ApplicationUser> userMgr)
    {
        var alice = await userMgr.FindByNameAsync("alice");
        if (alice != null)
        {
            await userMgr.DeleteAsync(alice);
        }
        Console.WriteLine("alice");
        alice = new ApplicationUser
        {
            UserName = "alice",
            Email = "AliceSmith@email.com",
            EmailConfirmed = true,
        };
        var result = await userMgr.CreateAsync(alice, "Pass123$");
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        await userMgr.AddToRoleAsync(alice, RoleAdmin);

        result = await userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com")
                        });
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }

    private static async Task CreateBobAsync(UserManager<ApplicationUser> userMgr)
    {
        var bob = await userMgr.FindByNameAsync("bob");
        if (bob != null)
        {
            await userMgr.DeleteAsync(bob);
        }
        bob = new ApplicationUser
        {
            UserName = "bob",
            Email = "BobSmith@email.com",
            EmailConfirmed = true
        };
        var result = await userMgr.CreateAsync(bob, "Pass123$");
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        result = await userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        });
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
        Log.Debug("bob created");
    }

    private static async Task CreateRolesAsync(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var adminRole = await RoleManager.Roles.FirstOrDefaultAsync(x => x.Name == RoleAdmin);

        if (adminRole != null)
        {
            await RoleManager.DeleteAsync(adminRole);
        }
        await RoleManager.CreateAsync(adminRole);
        await RoleManager.AddClaimAsync(adminRole, new Claim(Config.ForecastReadPermission, ""));
        await RoleManager.AddClaimAsync(adminRole, new Claim(Config.ForecastExternalSourcesPermission, ""));
    }
}
