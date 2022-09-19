using System.Security.Claims;
using IdentityModel;
using IDServer.Models;
using SharedProject.IdentityServer;

namespace IDServer.Data;

public static class TestUsers {
  public static List<TestUser> Users {
    get {
      return new List<TestUser> {
        new() {
          Name = "alice",
          User = new ApplicationUser {
            UserName = "alice",
            Email = "AliceSmith@email.com",
            EmailConfirmed = true
          },
          Claims = new Claim[] {
            new(JwtClaimTypes.Name, "Alice Smith"),
            new(JwtClaimTypes.GivenName, "Alice"),
            new(JwtClaimTypes.FamilyName, "Smith"),
            new(JwtClaimTypes.WebSite, "https://alice.com"),
            new(JwtClaimTypes.Role, Roles.User)
          },
          Scopes = new[] {
            Scopes.ApiScope.Name
          }
        },
        new() {
          Name = "bob",
          User = new ApplicationUser {
            UserName = "bob",
            Email = "BobSmith@email.com",
            EmailConfirmed = true
          },
          Claims = new Claim[] {
            new(JwtClaimTypes.Name, "Bob Smith"),
            new(JwtClaimTypes.GivenName, "Bob"),
            new(JwtClaimTypes.FamilyName, "Smith"),
            new(JwtClaimTypes.WebSite, "https://bob.com"),
            new("location", "somewhere"),
            new(JwtClaimTypes.Role, Roles.Admin)
          },
          Scopes = new[] {
            Scopes.ApiScope.Name
          }
        }
      };
    }
  }
}

public class TestUser {
  public string Name { get; set; }
  public ApplicationUser User { get; set; }
  public Claim[] Claims { get; set; }
  public string[] Scopes { get; set; }
  public string Password { get; set; } = "Pass123$";
}
//new(JwtClaimTypes.Scope, SharedProject.IdentityServer.Scopes.MVCScope)