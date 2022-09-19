using System.Security.Claims;
using IdentityModel;
using IDServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IDServer.Data;

public static class SeedData {
  public static void EnsureSeedData(WebApplication app) {
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    context.Database.Migrate();

    foreach (var testUser in TestUsers.Users){
      var user = userMgr.FindByNameAsync(testUser.User.UserName).Result;
      if (user is not null) userMgr.DeleteAsync(user);
      // if (user is not null){
      //   Log.Debug($"{testUser.User.UserName} already exists");
      //   continue;
      // }

      var scopeClaims = testUser.Scopes.Select(x => new Claim(JwtClaimTypes.Scope, x));
      var allClaims = testUser.Claims.Concat(scopeClaims);

      var result = userMgr.CreateAsync(testUser.User, testUser.Password).Result;
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      result = userMgr.AddClaimsAsync(testUser.User, allClaims).Result;
      if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

      Log.Debug($"{testUser.User.UserName} created");
    }
  }
}