// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IDServer;

public static class Config {
  public static IEnumerable<IdentityResource> IdentityResources =>
    new IdentityResource[] {
      new IdentityResources.OpenId(),
      new IdentityResources.Profile()
    };

  public static IEnumerable<ApiScope> ApiScopes =>
    new ApiScope[] {
      new("MVCScope")
    };

  public static IEnumerable<Client> Clients =>
    new Client[] {
      // interactive client using code flow + pkce
      new() {
        ClientId = "MVCID",
        ClientSecrets = {new Secret("MVCSecret".Sha256())},

        AllowedGrantTypes = GrantTypes.Code,

        // where to redirect to after login
        RedirectUris = {"https://localhost:5002/signin-oidc"},

        // where to redirect to after logout
        PostLogoutRedirectUris = {"https://localhost:5002/signout-callback-oidc"},

        AllowedScopes = new List<string> {
          "MVCScope",
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile
        },
        RequirePkce = true,
        AllowPlainTextPkce = false
      }
    };
}