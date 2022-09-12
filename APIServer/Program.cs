using System.Reflection;
using APIServer.Database;
using APIServer.Logging.Database;
using APIServer.Logging.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>();

//LIBRARY Logger
builder.Services.AddDbContext<LogDbContext>();

// var context = new LogDbContext(
//   new DbContextOptionsBuilder<LogDbContext>().UseSqlite($"Data Source={databasePath};").Options
// );
// context.Database.EnsureDeleted();
// context.Database.Migrate();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//LIBRARY Logger
app.UseRequestResponseLogging();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); //.RequireAuthorization();

app.Run();