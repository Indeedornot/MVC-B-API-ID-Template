using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Duende.IdentityServer;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MVCServer.Api;
using Refit;
using SharedProject.IdentityServer;
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

    builder.Services.AddHttpContextAccessor();

    //TODO ENSURE USER HAS APISCOPE
    builder.Services.AddTransient<HeaderHandler>();
    builder.Services.AddRefitClient<IWebApi>()
      .ConfigureHttpClient(c => {
        c.BaseAddress = new Uri(SharedProject.IpAddresses.APIServer);
      }).AddHttpMessageHandler<HeaderHandler>();

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


    app.UseRequestLocalization(); //Localization

    app.UseHttpsRedirection();
    app.UseBlazorFrameworkFiles(); // Blazor
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    //app.MapBffManagementEndpoints(); //ID4

    app.UseEndpoints(endpoints => {
      app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}"
      ).RequireAuthorization();
      endpoints.MapFallbackToFile("index.html");
    });

    return app;
  }

  private static IServiceCollection AddConfiguredIdentityServer(this IServiceCollection services) {
    services.AddAccessTokenManagement();

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
          options.Authority = SharedProject.IpAddresses.IdentityServer;

          options.ClientId = "MVCID";
          options.ClientSecret = "MVCSecret";
          options.ResponseType = "code";
          options.ResponseMode = "query";
          options.UsePkce = true;

          //TODO: Look into scope based authorization for certain pages
          options.Scope.Add(Scopes.ApiScope.Name);
          options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
          options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
          options.GetClaimsFromUserInfoEndpoint = true;
          options.MapInboundClaims = true;

          options.Scope.Add(Scopes.Roles.Name);
          options.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role);
          options.TokenValidationParameters = new TokenValidationParameters {
            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role
          };

          options.SignedOutRedirectUri = $"{SharedProject.IpAddresses.MVCServer}/signout-callback-oidc";

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