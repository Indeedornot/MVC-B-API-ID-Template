using IdentityServer4;

using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Logging;
using SharedProject.Localization;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//LIBRARY LoC Service
#region Localization
builder.Services.AddLocalization();
builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization(options => {
  options.DataAnnotationLocalizerProvider = (type, factory) =>
    factory.Create(typeof(SharedResource));
}); ;

builder.Services.Configure<RequestLocalizationOptions>(
  options => {
    var supportedCultures = new List<CultureInfo> {
      new("en-US"),
      new("de-CH"),
      new("fr-CH"),
      new("it-CH")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.SetDefaultCulture("en-US");

    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
  }
);
#endregion

builder.Services.AddHttpClient();

//LIBRARY IdentityServer

#region IdentityServer
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(
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
#endregion

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
  {
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
  }
else
  {
  app.UseDeveloperExceptionPage();

  //LIBRARY IdentityServer
  IdentityModelEventSource.ShowPII = true;

  //LIBRARY BWASM
  app.UseWebAssemblyDebugging();
  }

//LIBRARY BWASM
app.UseBlazorFrameworkFiles();

//LIBRARY Localization
app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints => {
  endpoints.MapControllers().RequireAuthorization();
}); //LIBRARY IdentityServer - authenticate all requests

app.Run();