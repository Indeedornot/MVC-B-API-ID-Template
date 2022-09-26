using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using SharedProject;
using SharedProject.IdentityServer;

namespace IDServer;

public static class Config {
  public static IEnumerable<IdentityResource> IdentityResources =>
    new IdentityResource[] {
      new IdentityResources.OpenId(),
      new IdentityResources.Profile(),
      Scopes.Roles
    };

  public static IEnumerable<ApiScope> ApiScopes =>
    new[] {Scopes.ApiScope};

  public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[] { };

  public static IEnumerable<Client> Clients =>
    new Client[] {
      // interactive client using code flow + pkce
      new() {
        ClientId = "MVCID",
        ClientSecrets = {new Secret("MVCSecret".Sha256())},

        AllowedGrantTypes = GrantTypes.Code,

        // where to redirect to after login
        RedirectUris = {$"{IpAddresses.MVCServer}/signin-oidc"},

        // where to redirect to after logout
        PostLogoutRedirectUris = {$"{IpAddresses.MVCServer}/signout-callback-oidc"},

        AllowedScopes = new List<string> {
          Scopes.ApiScope.Name,
          Scopes.Roles.Name,
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile
        },
        RequirePkce = true,
        AllowPlainTextPkce = false
      },
      new() {
        ClientId = "APISwagger",
        RequireClientSecret = false,
        AllowedGrantTypes = GrantTypes.Code,
        RedirectUris = {$"{IpAddresses.APIServer}/swagger/oauth2-redirect.html"},
        AllowedCorsOrigins = {IpAddresses.APIServer},
        AllowOfflineAccess = true,
        AllowedScopes = new List<string> {
          Scopes.ApiScope.Name,
          Scopes.Roles.Name,
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile
        },
        AllowAccessTokensViaBrowser = true
      },
      new() {
        ClientId = "POSTMAN",
        ClientSecrets = {new Secret("POSTMAN".Sha256())},

        AllowedGrantTypes = GrantTypes.Code,

        // where to redirect to after login
        RedirectUris = {"https://oauth.pstmn.io/v1/browser-callback"},

        // where to redirect to after logout
        PostLogoutRedirectUris = {$"{IpAddresses.MVCServer}/signout-callback-oidc"},

        AllowedScopes = new List<string> {
          Scopes.ApiScope.Name,
          Scopes.Roles.Name,
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile
        },
        RequirePkce = false,
        AllowPlainTextPkce = false,
        AllowAccessTokensViaBrowser = true
      }
    };
}