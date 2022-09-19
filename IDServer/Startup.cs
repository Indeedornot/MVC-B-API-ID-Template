using IDServer.Data;
using IDServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IDServer;

internal static class Startup {
  public static WebApplication ConfigureServices(this WebApplicationBuilder builder) {
    builder.Services.AddRazorPages();

    string dbPath = Path.Combine(builder.Environment.ContentRootPath, "Data/AspIdUsers.db");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlite($"Data Source={dbPath};"));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

    builder.Services
      .AddIdentityServer(options => {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

        // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
        options.EmitStaticAudienceClaim = true;
      })
      .AddInMemoryIdentityResources(Config.IdentityResources)
      .AddInMemoryApiScopes(Config.ApiScopes)
      .AddInMemoryClients(Config.Clients)
      .AddInMemoryApiResources(Config.ApiResources)
      .AddAspNetIdentity<ApplicationUser>()
      .AddProfileService<IdentityProfileService>();

    builder.Services.AddAuthentication();
    // .AddGoogle(options =>
    // {
    //     options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
    //
    //     // register your IdentityServer with Google at https://console.developers.google.com
    //     // enable the Google+ API
    //     // set the redirect URI to https://localhost:5001/signin-google
    //     options.ClientId = "copy client ID from Google here";
    //     options.ClientSecret = "copy client secret from Google here";
    // });

    return builder.Build();
  }

  public static WebApplication ConfigurePipeline(this WebApplication app) {
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment()){
      app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();
    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();

    app.MapRazorPages()
      .RequireAuthorization();

    return app;
  }
}