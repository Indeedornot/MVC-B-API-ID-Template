using APIServer.Database;
using APIServer.Logging.Database;
using APIServer.Logging.Middleware;
using Duende.IdentityServer;
using FastEndpoints;
using FastEndpoints.Swagger;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>();

//LIBRARY Logger
builder.Services.AddDbContext<LogDbContext>();

//LIBRARY IdentityServer

//Require certain token
builder.Services.AddAuthorization(options => {
  options.AddPolicy(
    SharedProject.IdentityServer.Scopes.ApiScope.Name,
    policy => {
      policy.RequireAuthenticatedUser();
      policy.RequireAssertion(context => {
        string DictToString(Dictionary<string, string> dict) {
          return string.Join(Environment.NewLine, dict.Select(x => $"{x.Key}: {x.Value}"));
        }

        Console.WriteLine(DictToString(context.User.Claims.ToDictionary(x => x.Type, x => x.Value)));
        return context.User.HasClaim(JwtClaimTypes.Scope, SharedProject.IdentityServer.Scopes.ApiScope.Name)
               && context.User.HasClaim(JwtClaimTypes.Role, SharedProject.IdentityServer.Roles.Admin);
      });
    }
  );
});

//Require any token from IdentityServer
builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer",
    options => {
      options.Authority = SharedProject.IpAddresses.IdentityServer;
      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateAudience = false
      };
    });

builder.Services.AddFastEndpoints();

//Add new auth option to the list
builder.Services.AddSwaggerDoc(
  s => {
    s.AddSecurity(
      "Authorization",
      Enumerable.Empty<string>(),
      new OpenApiSecurityScheme {
        Type = OpenApiSecuritySchemeType.OAuth2,
        Flow = OpenApiOAuth2Flow.AccessCode,
        Flows = new OpenApiOAuthFlows {
          AuthorizationCode = new OpenApiOAuthFlow {
            AuthorizationUrl = $"{SharedProject.IpAddresses.IdentityServer}/connect/authorize",
            TokenUrl = $"{SharedProject.IpAddresses.IdentityServer}/connect/token",
            Scopes = new Dictionary<string, string> {
              {SharedProject.IdentityServer.Scopes.ApiScope.Name, SharedProject.IdentityServer.Scopes.ApiScope.DisplayName},
              {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.OpenId},
              {SharedProject.IdentityServer.Scopes.Roles.Name, SharedProject.IdentityServer.Scopes.Roles.DisplayName}
            }
          }
        }
      }
    );

    s.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Authorization"));
  }
);

builder.Services.AddCoreAdmin("Admin");


var app = builder.Build();

app.UseRequestResponseLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

//LIBRARY Logger

app.UseRouting();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

//app.UseCoreAdminCustomUrl("AdminPanel");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
  app.UseOpenApi();
  app.UseSwaggerUi3(c => {
    c.OAuth2Client = new OAuth2ClientSettings {
      ClientId = "APISwagger",
      AppName = "Swagger UI",
      UsePkceWithAuthorizationCodeGrant = true
    };
  });
}

app.MapControllerRoute(
  "default",
  "{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization(SharedProject.IdentityServer.Scopes.ApiScope.Name);

app.Run();