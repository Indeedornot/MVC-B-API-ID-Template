using APIServer.Database;
using APIServer.Logging.Database;
using APIServer.Logging.Middleware;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>();

//LIBRARY Logger
builder.Services.AddDbContext<LogDbContext>();

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(); //add this

//LIBRARY IdentityServer

#region IdentityServer
builder.Services.AddAuthorization(
  options => {
    options.AddPolicy(
      "ApiScope",
      policy => {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "MVCScope");
      }
    );
  }
);

builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer(
    "Bearer",
    options => {
      options.Authority = "https://localhost:5001";

      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateAudience = false
      };
    }
  );
#endregion

var app = builder.Build();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
  app.UseOpenApi();
  app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

//LIBRARY Logger
app.UseRequestResponseLogging();


app.MapControllers();

app.Run();