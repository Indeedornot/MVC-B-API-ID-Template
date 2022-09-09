using System.IdentityModel.Tokens.Jwt;
using IdentityServer4;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//LIBRARY IdentityServer

#region IdentityServer
builder.Services.AddHttpClient();
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

      options.ClientId = "mvc";
      options.ClientSecret = "secret";
      options.ResponseType = "code";
      options.ResponseMode = "query";
      options.UsePkce = true;

      options.Scope.Add("api1");
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
if (!app.Environment.IsDevelopment()){
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
else{
  app.UseDeveloperExceptionPage();

  //LIBRARY IdentityServer
  IdentityModelEventSource.ShowPII = true;

  //LIBRARY BWASM
  app.UseWebAssemblyDebugging();
}

//LIBRARY BWASM
app.UseBlazorFrameworkFiles();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//LIBRARY IdentityServer
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
  "default",
  "{controller=Home}/{action=Index}/{id?}"
);
//.RequireAuthorization() //LIBRARY IdentityServer - authenticate all requests

app.Run();