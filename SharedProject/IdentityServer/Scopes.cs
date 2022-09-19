using Duende.IdentityServer.Models;
using IdentityModel;

namespace SharedProject.IdentityServer;

public static class Scopes {
  public static readonly ApiScope ApiScope = new() {
    Name = "APIScope",
    DisplayName = "API Scope",
    Description = "Scope used to access the API",
    UserClaims = {JwtClaimTypes.Role}
  };

  public static readonly IdentityResource Roles = new(
    "roles",
    userClaims: new[] {JwtClaimTypes.Role},
    displayName: "User role(s)"
  );
}