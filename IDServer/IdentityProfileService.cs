using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IDServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IDServer;

public class IdentityProfileService : IProfileService {
  private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
  private readonly UserManager<ApplicationUser> _userManager;

  public IdentityProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager) {
    _claimsFactory = claimsFactory;
    _userManager = userManager;
  }

  public async Task GetProfileDataAsync(ProfileDataRequestContext context) {
    string sub = context.Subject.GetSubjectId();
    var user = await _userManager.FindByIdAsync(sub);
    if (user == null) throw new ArgumentException($"User with subjectId: {sub} not found");

    var principal = await _claimsFactory.CreateAsync(user);
    var claims = principal.Claims.ToList();

    //Add more claims like this
    //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

    //TODO Stuck at adding claims to the context
    context.IssuedClaims = claims;
  }

  public async Task IsActiveAsync(IsActiveContext context) {
    string sub = context.Subject.GetSubjectId();
    var user = await _userManager.FindByIdAsync(sub);
    context.IsActive = user != null;
  }
}