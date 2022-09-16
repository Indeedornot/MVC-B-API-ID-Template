using APIServer.Database;
using APIServer.Logging.Database;
using APIServer.Logging.Middleware;
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
    "ApiScope",
    policy => {
      policy.RequireAuthenticatedUser();
      policy.RequireClaim("scope", "MVCScope");
    }
  );
});
builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer",
    options => {
      options.Authority = "https://localhost:5001";
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
            AuthorizationUrl = "https://localhost:5001/connect/authorize",
            TokenUrl = "https://localhost:5001/connect/token",
            Scopes = new Dictionary<string, string> {
              {"MVCScope", "MVCScope"}
            }
          }
        }
      }
    );

    s.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
  }
);

var app = builder.Build();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.UseHttpsRedirection();

//LIBRARY Logger
app.UseRequestResponseLogging();

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
  app.MapControllers();
}

app.Run();