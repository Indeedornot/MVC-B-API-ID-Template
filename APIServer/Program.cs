using APIServer.Database;
using APIServer.Logging.Database;
using APIServer.Logging.Middleware;
using Duende.IdentityServer;
using FastEndpoints;
using FastEndpoints.Swagger;
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

builder.Services.AddAuthorization(options => {
  options.AddPolicy(
    SharedProject.IdentityServer.Scopes.ApiScope.Name,
    policy => {
      policy.RequireAuthenticatedUser();
      //policy.RequireClaim(JwtClaimTypes.Scope, SharedProject.IdentityServer.Scopes.ApiScope.Name);
      // policy.RequireAssertion(context => {
      //   return context.User.HasClaim(JwtClaimTypes.Scope, SharedProject.IdentityServer.Scopes.ApiScope.Name);
      // });
    }
  );
});
builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer",
    options => {
      options.Authority = SharedProject.IpAddresses.IdentityServer;
      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateAudience = false
      };
    });

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(
  s => {
    s.AddSecurity(
      "bearer",
      Enumerable.Empty<string>(),
      new OpenApiSecurityScheme {
        Type = OpenApiSecuritySchemeType.OAuth2,
        Flow = OpenApiOAuth2Flow.Implicit,
        Flows = new OpenApiOAuthFlows {
          Implicit = new OpenApiOAuthFlow {
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

    s.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
  }
);

var app = builder.Build();

app.UseRequestResponseLogging();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

//LIBRARY Logger

app.UseFastEndpoints();

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

app.MapControllers();

app.Run();