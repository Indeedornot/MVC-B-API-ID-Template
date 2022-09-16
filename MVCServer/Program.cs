using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Logging;
using MVCServer.Api;
using Refit;
using SharedProject.Localization;

namespace MVCServer;

internal static class Program {
  public static void Main(string[] args) {
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.ConfigureServices().Build();
    app.Configure().Run();
  }

  private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) {
    builder.Services.AddConfiguredLocalization();
    builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization(
      options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
          factory.Create(typeof(SharedResource));
      }
    );

    builder.Services.AddHttpClient();

    builder.Services.AddConfiguredIdentityServer();
    builder.Services.AddRefitClient<IWebApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5003"))
      .AddUserAccessTokenHandler();

    builder.Services.AddSession();

    return builder;
  }

  private static WebApplication Configure(this WebApplication app) {
    if (!app.Environment.IsDevelopment()){
      app.UseExceptionHandler("/Home/Error");
      app.UseHsts();
    }
    else{
      app.UseDeveloperExceptionPage();
      app.UseWebAssemblyDebugging();

      IdentityModelEventSource.ShowPII = true;
    }

    app.UseBlazorFrameworkFiles(); // Blazor

    app.UseRequestLocalization(); //Localization

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapBffManagementEndpoints(); //ID4

    app.MapControllerRoute(
      "default",
      "{controller=Home}/{action=Index}/{id?}"
    ).RequireAuthorization();
    return app;
  }

  private static IServiceCollection AddConfiguredIdentityServer(this IServiceCollection services) {
    services.AddBff();

    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    services.AddAuthentication(
        options => {
          options.DefaultScheme = "Cookies";
          options.DefaultChallengeScheme = "oidc";
        }
      )
      .AddCookie("Cookies")
      .AddOpenIdConnect(
        "oidc",
        options => {
          options.Authority = "https://localhost:5001";

          options.ClientId = "MVCID";
          options.ClientSecret = "MVCSecret";
          options.ResponseType = "code";
          options.ResponseMode = "query";
          options.UsePkce = true;

          options.Scope.Add("MVCScope");
          options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
          options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);

          options.SignedOutRedirectUri = "https://localhost:5002/signout-callback-oidc";

          options.SaveTokens = true;
        }
      );
    return services;
  }

  private static IServiceCollection AddConfiguredLocalization(this IServiceCollection services) {
    services.AddLocalization();

    services.Configure<RequestLocalizationOptions>(
      options => {
        var supportedCultures = new List<CultureInfo> {
          new("en-US")
          // new("de-CH"),
          // new("fr-CH"),
          // new("it-CH")
        };

        options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.SetDefaultCulture("en-US");

        options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
      }
    );
    return services;
  }
}