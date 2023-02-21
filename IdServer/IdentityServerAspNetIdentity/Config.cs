using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServerAspNetIdentity;

public static class Config
{
    private const string ForecastReadScope = "forecasts.read";
    private const string ForecastLoadExternalSourcesScope = "forecasts.loadexternal";
    public const string ForecastReadPermission = "forecasts.read";
    public const string ForecastExternalSourcesPermission = "forecasts.externalsources";
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
               new ApiResource
               {
                   Scopes = new[] { ForecastReadScope, ForecastLoadExternalSourcesScope },
                   Name = "forecasts",
                   DisplayName= "Forecasts predictions",
                   UserClaims = new []{ ForecastReadPermission, ForecastExternalSourcesPermission }
               }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(ForecastReadScope),
            new ApiScope(ForecastLoadExternalSourcesScope)
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
          new Client
            {
                ClientId = "spa",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RedirectUris = { "https://localhost:44418/signin-callback" },
                PostLogoutRedirectUris = { "https://localhost:44418/signout-callback" },
                AllowedCorsOrigins =     { "https://localhost:44418" },
                AllowOfflineAccess= true,
                RequireConsent = true,
                // Add user infor to id token, usualy get by http call
                AlwaysIncludeUserClaimsInIdToken=true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                    ForecastReadScope,
                    ForecastLoadExternalSourcesScope,
                    "roles"
                }
            }
        };
}
